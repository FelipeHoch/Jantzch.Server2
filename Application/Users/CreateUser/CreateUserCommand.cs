using MediatR;

namespace Jantzch.Server2.Application.Users.CreateUser;

public record CreateUserCommand(string Data) : IRequest<UserResponse>;

