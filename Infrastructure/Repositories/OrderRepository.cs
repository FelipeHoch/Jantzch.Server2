using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Orders;
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
            var startDateFilter = builder.Gte(order => order.FinishedAt, paramaters.StartDate);
            var endDateFilter = builder.Lte(order => order.FinishedAt, paramaters.EndDate);

            filter &= startDateFilter & endDateFilter;
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
            var clientFilter = builder.Eq(order => order.Client.Name, paramaters.Client);
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

        if (!string.IsNullOrWhiteSpace(paramaters.OrderBy))
        {
            if (_propertyCheckerService.TypeHasProperties<Order>(paramaters.OrderBy))
            {
                sort = Builders<Order>.Sort.Descending(paramaters.OrderBy);
            }
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

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _orders.InsertOneAsync(order, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        await _orders.ReplaceOneAsync(x => x.Id == order.Id, order, cancellationToken: cancellationToken);
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
