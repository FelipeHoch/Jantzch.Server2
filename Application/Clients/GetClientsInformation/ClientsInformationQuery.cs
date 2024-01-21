using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Clients.GetClients;

public record ClientsInformationQuery(ClientsResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
