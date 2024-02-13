using MediatR;

namespace Jantzch.Server2.Application.Orders.Notifications.OrderCreated;

public class OrderCreatedNotification : INotification
{
    public OrderCreatedNotification(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}
