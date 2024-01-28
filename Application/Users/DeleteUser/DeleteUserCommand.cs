using MediatR;

namespace Jantzch.Server2.Application.Users.DeleteUser;

public record DeleteUserCommand(string Id) : IRequest;
