using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Application.Shared;
using Jantzch.Server2.Domain.Entities.Events;
using MongoDB.Driver;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class EventTypeRepository : IEventTypeRepository
{
    private readonly IMongoDatabase _database;

    private readonly IMongoCollection<EventType> _eventTypes;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public EventTypeRepository(IMongoDatabase database, IPropertyCheckerService propertyCheckerService)
    {
        _database = database;
        _eventTypes = _database.GetCollection<EventType>("event_types");
        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<EventType>> GetAsync(ResourceParameters parameters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var builder = Builders<EventType>.Filter;
        var filter = builder.Empty;
        var sort = Builders<EventType>.Sort.Ascending(e => e.Name);

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {
            var searchQuery = parameters.SearchQuery.Trim().ToLower();

            var searchFilter = builder.Where(e => e.Name.ToLower().Contains(searchQuery));

            filter &= searchFilter;
        }

        var eventTypes = _eventTypes.Aggregate()
            .Match(filter)
            .Sort(sort);

        var count = (int)await _eventTypes.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        
        return await PagedList<EventType>.CreateAsync(eventTypes, parameters.PageNumber, parameters.PageSize, count, cancellationToken);
    }

    public async Task<EventType?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        var eventType = await _eventTypes.Find(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);

        return eventType;
    }

    public async Task AddAsync(EventType eventType, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(eventType);

        await _eventTypes.InsertOneAsync(eventType, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(EventType eventType, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(eventType);

        await _eventTypes.ReplaceOneAsync(e => e.Id == eventType.Id, eventType, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(EventType eventType, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(eventType);

        await _eventTypes.DeleteOneAsync(e => e.Id == eventType.Id, cancellationToken: cancellationToken);
    }
}
