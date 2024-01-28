using Jantzch.Server2.Domain.Entities.Users;
using MediatR;

namespace Jantzch.Server2.Application.Users.CreateUser;

public record CreateUserCommand(string Data, User UserFromClient) : IRequest<UserResponse>;

