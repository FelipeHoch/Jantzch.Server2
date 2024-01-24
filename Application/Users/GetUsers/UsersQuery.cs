using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Users.GetUsers;

public record UsersQuery(UsersResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
