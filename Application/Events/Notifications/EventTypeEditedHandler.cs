using Jantzch.Server2.Domain.Entities.Events;
using MediatR;

namespace Jantzch.Server2.Application.Events.Notifications;

public class EventTypeEditedHandler : INotificationHandler<EventTypeEditedNotification>
{
    private readonly IEventRepository _eventRepository;

    public EventTypeEditedHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(EventTypeEditedNotification notification, CancellationToken cancellationToken)
    {
        await _eventRepository.UpdateManyEventType(notification.EventType, cancellationToken);
    }
}
