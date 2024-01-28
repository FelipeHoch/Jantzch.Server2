using Jantzch.Server2.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Jantzch.Server2.Application.Users.EditUser;

public class EditUserCommand
{
    public record Command(JsonPatchDocument<User> Model, string Id) : IRequest<User>;
}
