using Jantzch.Server2.Application.OrderReports.Models;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Orders;

namespace Jantzch.Server2.Application.OrderReports.PendingReports;

public class ClientWithPendingValue
{
    public ClientSimple Client { get; set; }

    public double PendingValue { get; set; }

    public List<OrderExport> OrdersToExport { get; set; } = [];
}
