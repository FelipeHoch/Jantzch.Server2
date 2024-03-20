using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Application.Services.PropertyChecker;
using MongoDB.Driver;

namespace Jantzch.Server2;

public class PotentialOrderRepository : IPotentialOrderRepository
{
    private readonly IMongoDatabase _database;

    private readonly IMongoCollection<PotentialOrder> _potentialOrders;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public PotentialOrderRepository(IMongoDatabase database, IPropertyCheckerService propertyCheckerService)
    {
        _database = database;
        _potentialOrders = _database.GetCollection<PotentialOrder>("potential_orders");
        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<PotentialOrder>> GetAsync(PotentialOrderResourceParameters parameters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var builder = Builders<PotentialOrder>.Filter;
        var filter = builder.Empty;
        var sort = Builders<PotentialOrder>.Sort.Descending(order => order.CreatedAt);

        if (!string.IsNullOrWhiteSpace(parameters.CreatedBy))
        {
            var createdByFilter = builder.Eq(order => order.CreatedBy.Name, parameters.CreatedBy);
            filter &= createdByFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.Status))
        {
            PotentialOrderStatus[] statusSplited = parameters.Status.Split(',')
            .Select(s => (PotentialOrderStatus)Enum.Parse(typeof(PotentialOrderStatus), s))
            .ToArray();

            List<FilterDefinition<PotentialOrder>> statusFilterList = [];

            foreach (var status in statusSplited)
            {
                statusFilterList.Add(builder.Eq(order => order.Status, status));
            }

            var statusFilter = builder.Or(statusFilterList);
            filter &= statusFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.Client))
        {
            var clientFilter = builder.Eq(order => order.Client.Name, parameters.Client);
            filter &= clientFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            if (_propertyCheckerService.TypeHasProperties<PotentialOrder>(parameters.OrderBy))
            {
                sort = Builders<PotentialOrder>.Sort.Descending(parameters.OrderBy);
            }
        }

        var potentialOrders = _potentialOrders.Aggregate()
            .Match(filter)
            .Sort(sort);

        var count = (int)await _potentialOrders.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<PotentialOrder>.CreateAsync(potentialOrders, parameters.PageNumber, parameters.PageSize, count, cancellationToken);
    }

    public async Task<PotentialOrder?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await _potentialOrders.Find(order => order.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(PotentialOrder order, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);

        await _potentialOrders.InsertOneAsync(order, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(PotentialOrder order, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);

        await _potentialOrders.ReplaceOneAsync(o => o.Id == order.Id, order, cancellationToken: cancellationToken);
    }
}
