using MediatR;

namespace Jantzch.Server2.Application.OrderReports.GetExportByOrder;

public record ReportByOrderQuery(string ClientId, string OrderId, string? TaxesId) : IRequest<OrderReportResponse>;
