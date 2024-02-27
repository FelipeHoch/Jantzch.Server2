using Jantzch.Server2.Domain.Entities.Events;
using MediatR;

namespace Jantzch.Server2.Application.Events.Notifications;

public class EventTypeDeletedHandler : INotificationHandler<EventTypeDeletedNotification>
{
    private readonly IEventRepository _eventRepository;

    public EventTypeDeletedHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(EventTypeDeletedNotification notification, CancellationToken cancellationToken)
    {
        var parameters = new EventResourceParameters
        {
            EventType = notification.EventType.Name
        };

        var events = await _eventRepository.GetAsync(parameters, cancellationToken);

        await _eventRepository.DeleteManyAsync(events, cancellationToken);
    }
}
