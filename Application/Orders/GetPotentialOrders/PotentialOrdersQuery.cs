using System.Dynamic;
using Jantzch.Server2.Application.Orders;
using MediatR;

namespace Jantzch.Server2;

public record PotentialOrdersQuery(PotentialOrderResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
