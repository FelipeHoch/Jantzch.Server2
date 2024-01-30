using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.Orders.CreateBreak;

public class CreateBreakCommandHandler : IRequestHandler<CreateBreakCommand.Command, Order>
{
    private readonly IOrderRepository _orderRepository;

    public CreateBreakCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> Handle(CreateBreakCommand.Command request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null) throw new RestException(HttpStatusCode.NotFound, new { Order = "Not found" });

        // TODO: Get data from token
        order.PauseOrder(request.Descriptive, "64a49b23141f6f29641cc4ce", "Felipe Mock");

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order;
    }
}
