using Jantzch.Server2.Domain.Entities.Events;
using MediatR;

namespace Jantzch.Server2.Application.Events.Notifications;

public class EventTypeEditedNotification : INotification
{
    public EventTypeEditedNotification(EventType eventType)
    {
        EventType = eventType;
    }

    public EventType EventType { get; set; }
}
