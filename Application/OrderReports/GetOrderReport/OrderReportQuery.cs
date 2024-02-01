using MediatR;

namespace Jantzch.Server2.Application.OrderReports.GetOrderReport;

public record OrderReportQuery(string ClientId, string OrderId) : IRequest<OrderReportResponse>;
