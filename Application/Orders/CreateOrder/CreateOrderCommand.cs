using Domain.Entities.Orders.Enums;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;

namespace Jantzch.Server2.Application.Orders.CreateOrder;

public class CreateOrderCommand : IRequest<Order>
{
    public int? OrderNumber { get; set; } = 0;

    public string? Observations { get; set; }

    public ClientSimple Client { get; set; }

    public int PredictedTime { get; set; } = 0;

    public DateTime? ScheduledDate { get; set; }

    public List<UserSimple> Workers { get; set; } = [];

    public OrderType Type { get; set; }
}
