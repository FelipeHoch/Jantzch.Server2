using Jantzch.Server2.Domain.Entities.Events;
using MediatR;

namespace Jantzch.Server2.Application.Events.CreateEventType;

public class CreateEventTypeCommand : IRequest<EventType>
{
    public string Name { get; set; } = string.Empty;

    public string HexColor { get; set; } = string.Empty;
}
