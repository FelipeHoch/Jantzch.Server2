using Jantzch.Server2.Application.Events;
using Jantzch.Server2.Application.Helpers;

namespace Jantzch.Server2.Domain.Entities.Events;

public interface IEventRepository
{
    Task <PagedList<Event>> GetAsync(EventResourceParameters parameters, CancellationToken cancellationToken);

    Task<Event?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task AddAsync(Event @event, CancellationToken cancellationToken);

    Task AddManyAsync(List<Event> events, CancellationToken cancellationToken);

    Task UpdateAsync(Event @event, CancellationToken cancellationToken);

    Task UpdateManyEventType(EventType eventType, CancellationToken cancellationToken);

    Task DeleteAsync(Event @event, CancellationToken cancellationToken);

    Task DeleteManyAsync(List<Event> events, CancellationToken cancellationToken);
}
