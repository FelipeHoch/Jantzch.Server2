using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Clients.GetClients;

public record ClientsQuery(ClientsResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
