using MediatR;

namespace Jantzch.Server2.Application.Events.DeleteEventType;

public record DeleteEventTypeCommand(string Id) : IRequest;
