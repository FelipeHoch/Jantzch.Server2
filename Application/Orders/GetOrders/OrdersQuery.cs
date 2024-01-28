using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Orders.GetOrders;

public record OrdersQuery(OrderResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
