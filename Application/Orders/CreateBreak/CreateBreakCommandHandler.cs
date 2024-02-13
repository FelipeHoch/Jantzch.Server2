using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Orders.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.Orders.CreateBreak;

public class CreateBreakCommandHandler : IRequestHandler<CreateBreakCommand.Command, Order>
{
    private readonly IOrderRepository _orderRepository;

    private readonly IJwtService _jwtService;

    public CreateBreakCommandHandler(IOrderRepository orderRepository, IJwtService jwtService)
    {
        _orderRepository = orderRepository;

        _jwtService = jwtService;
    }

    public async Task<Order> Handle(CreateBreakCommand.Command request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null) throw new RestException(HttpStatusCode.NotFound, new { message = OrdersErrorMessages.NOT_FOUND });

        order.PauseOrder(request.Descriptive, _jwtService.GetSubFromToken(), _jwtService.GetNameFromToken());

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order;
    }
}
