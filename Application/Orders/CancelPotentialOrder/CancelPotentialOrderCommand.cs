using MediatR;

namespace Jantzch.Server2;

public record CancelPotentialOrderCommand(string Id) : IRequest;
