using Jantzch.Server2.Application.Events;
using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Events;
using MongoDB.Driver;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly IMongoDatabase _database;

    private readonly IMongoCollection<Event> _events;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public EventRepository(IMongoDatabase database, IPropertyCheckerService propertyCheckerService)
    {
        _database = database;
        _events = _database.GetCollection<Event>("events");
        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<Event>> GetAsync(EventResourceParameters parameters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var builder = Builders<Event>.Filter;
        var filter = builder.Empty;
        var sort = Builders<Event>.Sort.Descending(e => e.StartDate);

        if (parameters.StartDate is not null && parameters.EndDate is not null)
        {
            var startDateFilter = builder.Gte(e => e.StartDate, parameters.StartDate);
            var endDateFilter = builder.Lte(e => e.StartDate, parameters.EndDate);

            filter &= startDateFilter & endDateFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.EventType))
        {
            var typeFilter = builder.Eq(e => e.EventType.Name, parameters.EventType);
            filter &= typeFilter;
        }

        if (!string.IsNullOrEmpty(parameters.UserIds))
        {
            var userIds = parameters.UserIds.Split(',');

            var userIdsFilter = builder.In(e => e.User.Id, userIds);

            filter &= userIdsFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {
            var searchQuery = parameters.SearchQuery.Trim().ToLower();

            var where = builder.Or(
                 builder.Regex(order => order.Name, "/^" + searchQuery + "/i"),
                 builder.Regex(order => order.User.Name, "/^" + searchQuery + "/i")            
                );

            filter &= where;
        }


        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            if (_propertyCheckerService.TypeHasProperties<Event>(parameters.OrderBy))
            {
                sort = Builders<Event>.Sort.Descending(parameters.OrderBy);
            }        
        }

        var events = _events.Aggregate()
            .Match(filter)
            .Sort(sort);

        var count = (int)await _events.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<Event>.CreateAsync(events, parameters.PageNumber, parameters.PageSize, count, cancellationToken);
    }

    public async Task<PagedList<Event>> GetByUserAsync(string userId, EventResourceParameters parameters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(parameters);

        var builder = Builders<Event>.Filter;
        var filter = builder.Eq(e => e.User.Id, userId);
        var sort = Builders<Event>.Sort.Descending(e => e.StartDate);

        if (parameters.StartDate is not null && parameters.EndDate is not null)
        {
            var startDateFilter = builder.Gte(e => e.StartDate, parameters.StartDate);
            var endDateFilter = builder.Lte(e => e.StartDate, parameters.EndDate);

            filter &= startDateFilter & endDateFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.EventType))
        {
            var typeFilter = builder.Eq(e => e.EventType.Name, parameters.EventType);
            filter &= typeFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {
            var searchQuery = parameters.SearchQuery.Trim().ToLower();

            var where = builder.Or(
                 builder.Regex(order => order.Name, "/^" + searchQuery + "/i"),
                 builder.Regex(order => order.User.Name, "/^" + searchQuery + "/i")            
                );

            filter &= where;
        }

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            if (_propertyCheckerService.TypeHasProperties<Event>(parameters.OrderBy))
            {
                sort = Builders<Event>.Sort.Descending(parameters.OrderBy);
            }        
        }

        var events = _events.Aggregate()
            .Match(filter)
            .Sort(sort);

        var count = (int)await _events.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<Event>.CreateAsync(events, parameters.PageNumber, parameters.PageSize, count, cancellationToken);
    }

    public async Task<Event?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await _events.Find(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Event @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        await _events.InsertOneAsync(@event, cancellationToken: cancellationToken);
    }

    public async Task AddManyAsync(List<Event> events, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(events);

        await _events.InsertManyAsync(events, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Event @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        await _events.ReplaceOneAsync(e => e.Id == @event.Id, @event, cancellationToken: cancellationToken);
    }

    public async Task UpdateManyEventType(EventType eventType, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(eventType);

        await _events.UpdateManyAsync(e => e.EventType.Id == eventType.Id, Builders<Event>.Update.Set(e => e.EventType, eventType), cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Event @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        await _events.DeleteOneAsync(e => e.Id == @event.Id, cancellationToken: cancellationToken);
    }

    public async Task DeleteManyAsync(List<Event> events, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(events);

        var ids = events.Select(e => e.Id).ToList();

        await _events.DeleteManyAsync(e => ids.Contains(e.Id), cancellationToken: cancellationToken);
    }
}
