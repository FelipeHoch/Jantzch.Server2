using Jantzch.Server2.Domain.Entities.Events;
using MediatR;

namespace Jantzch.Server2.Application.Events.Notifications;

public class EventTypeDeletedNotification : INotification
{
    public EventTypeDeletedNotification(EventType eventType)
    {
        EventType = eventType;
    }

    public EventType EventType { get; set; }
}
