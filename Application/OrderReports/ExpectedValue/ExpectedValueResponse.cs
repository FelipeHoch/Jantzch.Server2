using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Domain.Entities.Clients;

namespace Jantzch.Server2.Application.OrderReports.ExpectedValue;

public class ExpectedValueResponse
{
    public double? ExpectedValue { get; set; }

    public ClientSimple Client { get; set; }

    public List<OrderResponse> Orders { get; set; } = [];
}
