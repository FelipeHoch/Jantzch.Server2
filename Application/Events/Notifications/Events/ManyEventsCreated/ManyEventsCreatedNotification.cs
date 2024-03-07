using Jantzch.Server2.Domain.Entities.Events;
using MediatR;

namespace Jantzch.Server2;

public class ManyEventsCreatedNotification : INotification
{
    public ManyEventsCreatedNotification(List<Event> events)
    {
        Events = events;
    }

    public List<Event> Events { get; set; }
}
