using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities.Orders.Enums;
using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.OrderReports.Models;
using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Infraestructure.Services.PropertyChecker;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoDatabase _database;

    private readonly IMongoCollection<Order> _orders;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public OrderRepository(IMongoDatabase database, IPropertyCheckerService propertyCheckerService)
    {
        _database = database;
        _orders = _database.GetCollection<Order>("orders");
        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<Order>> GetAsync(OrderResourceParameters paramaters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(paramaters);

        var builder = Builders<Order>.Filter;
        var filter = builder.Empty;
        var sort = Builders<Order>.Sort.Descending(order => order.OrderNumber);

        if (paramaters.StartDate is not null && paramaters.EndDate is not null)
        {
            var startDateFilter = builder.And(
                builder.Gte(order => order.StartDate, paramaters.StartDate),
                builder.Lte(order => order.StartDate, paramaters.EndDate)
            );

            var scheduledDateFilter = builder.And(
                builder.Gte(order => order.ScheduledDate, paramaters.StartDate),
                builder.Lte(order => order.ScheduledDate, paramaters.EndDate)
            );

            var dateFilter = builder.Or(startDateFilter, scheduledDateFilter);
            filter &= dateFilter;
        }

        if (!string.IsNullOrWhiteSpace(paramaters.CreatedBy))
        {
            var createdByFilter = builder.Eq(order => order.CreatedBy.Name, paramaters.CreatedBy);
            filter &= createdByFilter;
        }

        if (!string.IsNullOrWhiteSpace(paramaters.Status))
        {
            var statusSplited = paramaters.Status.Split(',');

            List<FilterDefinition<Order>> statusFilterList = [];

            foreach (var status in statusSplited)
            {
                statusFilterList.Add(builder.Eq(order => order.Status, status));
            }

            var statusFilter = builder.Or(statusFilterList);
            filter &= statusFilter;
        }

        if (!string.IsNullOrWhiteSpace(paramaters.Client))
        {
            var clientFilter = builder.Eq(order => order.Client.Id, paramaters.Client);
            filter &= clientFilter;
        }

        if (!string.IsNullOrWhiteSpace(paramaters.SearchQuery))
        {
            var searchQuery = paramaters.SearchQuery.Trim().ToLower();

            var where = builder.Or(
                 builder.Regex(order => order.CreatedBy, "/^" + searchQuery + "/i"),
                 builder.Regex(order => order.Client.Name, "/^" + searchQuery + "/i"),
                 builder.ElemMatch(order => order.Workers, worker => worker.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
            );
            filter &= where;
        }

        if (paramaters.OrderBy is not null)
        {
            if (_propertyCheckerService.TypeHasProperties<Order>(paramaters.OrderBy))
            {
                sort = paramaters.OrderByDesc is true
                    ? Builders<Order>.Sort.Descending(paramaters.OrderBy)
                    : Builders<Order>.Sort.Ascending(paramaters.OrderBy);
            }
        }

        if (!string.IsNullOrWhiteSpace(paramaters.Types))
        {
            var types = paramaters.Types.Split(',')
            .Select(type => (OrderType)Enum.Parse(typeof(OrderType), type))
            .ToArray();

            var typeFilter = builder.In(order => order.Type, types);
            filter &= typeFilter;
        }

        var orders = _orders.Aggregate()
            .Match(filter)
            .Sort(sort);

        var count = (int)await _orders.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<Order>.CreateAsync(orders, paramaters.PageNumber, paramaters.PageSize, count, cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _orders.Find(order => order.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<DetailedOrderForExport>> GetToExport(List<string> ordersId)
    {
        var ordersIdParsed = ordersId.Select(ObjectId.Parse).ToList();

        var filter = new BsonDocument("_id", new BsonDocument("$in", new BsonArray(ordersIdParsed)));

        var pipeline = new BsonDocument[]
        {
            new("$match", filter),

            new("$lookup",
                new BsonDocument("from", "users")
                    .Add("localField", "workers._id")
                    .Add("foreignField", "_id")
                    .Add("as", "workersFull")),

            new("$lookup",
                new BsonDocument("from", "materials")
                    .Add("localField", "materialsUsed.materialId")
                    .Add("foreignField", "_id")
                    .Add("as", "materialsFull")),

            new("$lookup",
                new BsonDocument("from", "clients")
                    .Add("localField", "client._id")
                    .Add("foreignField", "_id")
                    .Add("as", "client")),

            new("$unwind", "$client"),

            new("$project",
                new BsonDocument
                {
                    { "createdBy", 1 },
                    { "startDate", 1 },
                    { "finishedAt", 1 },
                    { "descriptive", 1 },
                    { "client", 1 },
                    { "breaksHistory", 1 },
                    { "orderNumber", 1 },
                    { "hoursWorked", 1 },
                    { "materialsUsed", 1 },
                    { "materials",
                        new BsonDocument("$map",
                            new BsonDocument
                            {
                                { "input", "$materialsFull" },
                                { "as", "m" },
                                { "in", new BsonDocument
                                    {
                                        { "_id", "$$m._id" },
                                        { "name", "$$m.name" },
                                        { "value", "$$m.value" }
                                    }
                                }
                            }
                        )
                    },
                    { "workers",
                        new BsonDocument("$map",
                            new BsonDocument
                            {
                                { "input", "$workersFull" },
                                { "as", "w" },
                                { "in", new BsonDocument
                                    {
                                        { "name", "$$w.name" },
                                        { "custByHour", "$$w.custByHour" }
                                    }
                                }
                            }
                        )
                    }
                }
            ),
            new("$sort", new BsonDocument("startDate", -1))
            };

        return await _orders.Aggregate<DetailedOrderForExport>(pipeline).ToListAsync();
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _orders.InsertOneAsync(order, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        await _orders.ReplaceOneAsync(x => x.Id == order.Id, order, cancellationToken: cancellationToken);
    }

    public async Task UpdateToReportedAsync(string[] ids, CancellationToken cancellationToken)
    {
        var ordersIdParsed = ids.Select(el => ObjectId.Parse(el)).ToArray();

        var filter = Builders<Order>.Filter.In("_id", ordersIdParsed);

        var update = Builders<Order>.Update.Set(o => o.IsReported, true);

        await _orders.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task UpdateToNoReportedAsync(string[] ids, CancellationToken cancellationToken)
    {
        var ordersIdParsed = ids.Select(el => ObjectId.Parse(el)).ToArray();

        var filter = Builders<Order>.Filter.In("_id", ordersIdParsed);

        var update = Builders<Order>.Update.Set(o => o.IsReported, false);

        await _orders.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Order order, CancellationToken cancellationToken)
    {
        await _orders.DeleteOneAsync(x => x.Id == order.Id, cancellationToken: cancellationToken);
    }

    public async Task<Order> LastOrderInserted(CancellationToken cancellationToken)
    {
        return await _orders.Find(order => true).SortByDescending(order => order.Id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<long> CountOrders(CancellationToken cancellationToken)
    {
        return await _orders.CountDocumentsAsync(order => true, cancellationToken: cancellationToken);
    }
}
