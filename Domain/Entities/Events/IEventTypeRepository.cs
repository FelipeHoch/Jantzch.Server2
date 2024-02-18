using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Shared;

namespace Jantzch.Server2.Domain.Entities.Events;

public interface IEventTypeRepository
{
    Task<PagedList<EventType>> GetAsync(ResourceParameters parameters, CancellationToken cancellationToken);

    Task<EventType?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task AddAsync(EventType eventType, CancellationToken cancellationToken);

    Task UpdateAsync(EventType eventType, CancellationToken cancellationToken);

    Task DeleteAsync(EventType eventType, CancellationToken cancellationToken);
}
