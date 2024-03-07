using MediatR;

namespace Jantzch.Server2;

public record DeleteEventCommand(string Id) : IRequest;