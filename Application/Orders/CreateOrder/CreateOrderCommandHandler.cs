using AutoMapper;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;

namespace Jantzch.Server2.Application.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;

    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;

        _mapper = mapper;
    }

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var lastOrder = await _orderRepository.LastOrderInserted(cancellationToken);

        request.OrderNumber = lastOrder?.OrderNumber + 1 ?? 1;

        var order = _mapper.Map<Order>(request);

        order.SetStatusOnCreation();

        // TODO: Get From token
        order.CreatedBy = new UserSimple
        {
            Id = "64a49b23141f6f29641cc4ce",
            Name = "Felipe Mock"
        };

        await _orderRepository.AddAsync(order, cancellationToken);

        return order;
    }
}
