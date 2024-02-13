using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Jantzch.Server2.Application.Orders.Notifications.OrderCreated;

public class OrderCreatedNotificationHandler : INotificationHandler<OrderCreatedNotification>
{
    private readonly IHubContext<OrderHub> _hubContext;

    private readonly IOrderRepository _orderRepository;

    private readonly IUserRepository _userRepository;

    public OrderCreatedNotificationHandler(IHubContext<OrderHub> hubContext, IOrderRepository orderRepository, IUserRepository userRepository)
    {
        _hubContext = hubContext;

        _orderRepository = orderRepository;

        _userRepository = userRepository;
    }

    public async Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(notification.Id, cancellationToken);

        var usersRelatedToOrder = order.Workers.Select(u => u.Id.ToString()).ToList();        

        var users = await _userRepository.GetByIdsAsync(usersRelatedToOrder, cancellationToken);

        var usersIds = users.Select(u => u.IdentityProviderId.ToString()).ToList();

        usersIds.Add(order.CreatedBy.Id);

        usersIds = usersIds.Distinct().ToList();
        
        await _hubContext.Clients.Users(usersIds).SendAsync("ReceiveOrder", order, cancellationToken: cancellationToken);
    }
}
