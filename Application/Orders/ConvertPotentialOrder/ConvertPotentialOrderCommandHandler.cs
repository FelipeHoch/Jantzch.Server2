using System.Net;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Application.Orders.Notifications.OrderCreated;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2;

public class ConvertPotentialOrderCommandHandler : IRequestHandler<ConvertPotentialOrderCommand, Order>
{
    private readonly IPotentialOrderRepository _potentialOrderRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IJwtService _jwtService;
    private readonly IMediator _mediator;

    public ConvertPotentialOrderCommandHandler(
        IPotentialOrderRepository potentialOrderRepository, 
        IOrderRepository orderRepository,
        IJwtService jwtService,
        IMediator mediator)
    {
        _potentialOrderRepository = potentialOrderRepository;
        _orderRepository = orderRepository;
        _jwtService = jwtService;
        _mediator = mediator;
    }

    public async Task<Order> Handle(ConvertPotentialOrderCommand request, CancellationToken cancellationToken)
    {
        var potentialOrder = await _potentialOrderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (potentialOrder is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = PotentialOrderErrorMessages.NOT_FOUND });
        }

        if (potentialOrder.IsCancelled()) {
            throw new RestException(HttpStatusCode.BadRequest, new { message = PotentialOrderErrorMessages.CANCELLED });
        }

        UserSimple convertedBy = new()
        {
            Id = _jwtService.GetNameIdentifierFromToken(),
            Name = _jwtService.GetNameFromToken()
        };

        var lastOrder = await _orderRepository.LastOrderInserted(cancellationToken);

        var orderNumber = lastOrder?.OrderNumber + 1 ?? 1;

        var order = potentialOrder.ConvertToOrder(convertedBy, orderNumber);

        await _orderRepository.AddAsync(order, cancellationToken);

        potentialOrder.RelateOrder(order.Id);

        await _potentialOrderRepository.UpdateAsync(potentialOrder, cancellationToken);

        await _mediator.Publish(new OrderCreatedNotification(order.Id), cancellationToken);

        return order;
    }
}
