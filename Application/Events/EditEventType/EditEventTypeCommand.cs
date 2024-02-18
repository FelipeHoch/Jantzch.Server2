using Jantzch.Server2.Domain.Entities.Events;
using MediatR;

namespace Jantzch.Server2.Application.Events.EditEventType;

public class EditEventTypeCommand
{
    public string Name { get; set; } = string.Empty;

    public string HexColor { get; set; } = string.Empty;

    public record Command(string Id, EditEventTypeCommand Model) : IRequest<EventType>;
}
