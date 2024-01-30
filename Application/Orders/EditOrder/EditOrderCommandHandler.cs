using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.Orders.EditOrder;

public class EditOrderCommandHandler : IRequestHandler<EditOrderCommand.Command, Order>
{
    private readonly IOrderRepository _orderRepository;

    public EditOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> Handle(EditOrderCommand.Command request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null) throw new RestException(HttpStatusCode.NotFound, new { Order = "Not found" });
        
        request.Model.ApplyTo(order);

        var status = request.Model.Operations.FirstOrDefault(x => x.path == "/status");

        if (status is not null && status.value is "Finalizada")
            // TODO: Get data from token
            order.FinishOrder("64a49b23141f6f29641cc4ce", "Felipe Mock");
                  
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order;
    }
}
