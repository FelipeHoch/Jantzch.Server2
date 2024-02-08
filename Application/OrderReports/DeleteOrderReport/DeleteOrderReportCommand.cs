using MediatR;

namespace Jantzch.Server2.Application.OrderReports.DeleteOrderReport;

public record DeleteOrderReportCommand(string Id) : IRequest;