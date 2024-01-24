using MediatR;

namespace Jantzch.Server2.Application.Clients.DeleteClient;

public record DeleteClientCommand(string Id) : IRequest;
