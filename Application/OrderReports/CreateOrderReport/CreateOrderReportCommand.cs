using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;

namespace Jantzch.Server2.Application.OrderReports.CreateOrderReport;

public class CreateOrderReportCommand
{
    public List<string> OrdersId { get; set; } = [];

    public List<string>? TaxesId { get; set; }

    public record Command(CreateOrderReportCommand Model, string ClientId) : IRequest<OrderReport>;
}
