using MediatR;

namespace Jantzch.Server2.Application.Orders.DeleteOrder;

public record DeleteOrderCommand(string Id): IRequest;
