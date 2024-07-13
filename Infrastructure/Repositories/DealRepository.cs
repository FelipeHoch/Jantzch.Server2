using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using MongoDB.Driver;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class DealRepository(
    IMongoDatabase database, 
    IPropertyCheckerService propertyCheckerService
) : IDealRepository
{
    private readonly IMongoCollection<Deal> _deals = database.GetCollection<Deal>("deals");

    public async Task<PagedList<Deal>> GetAsync(DealsResourceParamenters parameters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var filter = Builders<Deal>.Filter.Empty;
        var sort = Builders<Deal>.Sort.Descending(deal => deal.CreatedAt);

        if (!string.IsNullOrWhiteSpace(parameters.ClientId))
        {
            filter = filter & Builders<Deal>.Filter.Eq(deal => deal.Client.Id, parameters.ClientId);
        }

        if (parameters.Status is not null)
        {
            filter = filter & Builders<Deal>.Filter.Eq(deal => deal.Status, parameters.Status);
        }

        if (parameters.StartDate is not null)
        {
            filter = filter & Builders<Deal>.Filter.Gte(deal => deal.CreatedAt, parameters.StartDate);
        }

        if (parameters.EndDate is not null)
        {
            filter = filter & Builders<Deal>.Filter.Lte(deal => deal.CreatedAt, parameters.EndDate);
        }

        if (parameters.SearchQuery is not null)
        {
            var searchQuery = parameters.SearchQuery.Trim().ToLower();

            filter = filter & Builders<Deal>.Filter.Or(
                Builders<Deal>.Filter.Regex(deal => deal.Type, "/^" + parameters + "/i"),
                Builders<Deal>.Filter.Regex(deal => deal.Description, "/^" + parameters + "/i"),
                Builders<Deal>.Filter.Regex(deal => deal.Client.Name, "/^" + parameters + "/i")                
            );
        }

        if (parameters.OrderBy is not null)
        {
            if (propertyCheckerService.TypeHasProperties<Deal>(parameters.OrderBy))
            {
                sort = Builders<Deal>.Sort.Descending(parameters.OrderBy);
            }
        }

        var deals = _deals.Aggregate()
            .Match(filter)
            .Sort(sort);

        var total = (int)await _deals.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<Deal>.CreateAsync(deals, parameters.PageNumber, parameters.PageSize, total, cancellationToken);
    }

    public async Task<Deal?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _deals.Find(deal => deal.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Deal?> GetByIntegrationIdAsync(string integrationId, CancellationToken cancellationToken)
    {
        return await _deals.Find(deal => deal.IntegrationId == integrationId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Deal>> GetByIntegrationIdsAsync(List<string> integrationIds, CancellationToken cancellationToken)
    {
        var filter = Builders<Deal>.Filter.In(deal => deal.IntegrationId, integrationIds);

        return await _deals.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Deal deal, CancellationToken cancellationToken)
    {
        await _deals.InsertOneAsync(deal, cancellationToken: cancellationToken);
    }

    public async Task AddAsync(IEnumerable<Deal> deals, CancellationToken cancellationToken)
    {
        await _deals.InsertManyAsync(deals, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Deal deal, CancellationToken cancellationToken)
    {
        await _deals.ReplaceOneAsync(x => x.Id == deal.Id, deal, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        await _deals.DeleteOneAsync(deal => deal.Id == id, cancellationToken);
    }
}
