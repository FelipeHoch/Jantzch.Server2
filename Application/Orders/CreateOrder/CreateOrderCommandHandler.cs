using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Application.Orders.Notifications.OrderCreated;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Jantzch.Server2.Application.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;

    private readonly IJwtService _jwtService;

    private readonly IMapper _mapper;

    private readonly IHubContext<OrderHub> _hubContext;

    private readonly IMediator _mediator;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IJwtService jwtService, IHubContext<OrderHub> hub, IMediator mediator)
    {
        _orderRepository = orderRepository;

        _mapper = mapper;

        _jwtService = jwtService;

        _hubContext = hub;

        _mediator = mediator;
    }

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var lastOrder = await _orderRepository.LastOrderInserted(cancellationToken);

        request.OrderNumber = lastOrder?.OrderNumber + 1 ?? 1;

        var order = _mapper.Map<Order>(request);

        order.SetStatusOnCreation();

        order.CreatedBy = new UserSimple
        {
            Id = _jwtService.GetNameIdentifierFromToken(),
            Name = _jwtService.GetNameFromToken()
        };

        await _orderRepository.AddAsync(order, cancellationToken);

        await _mediator.Publish(new OrderCreatedNotification(order.Id), cancellationToken);

        return order;
    }
}
