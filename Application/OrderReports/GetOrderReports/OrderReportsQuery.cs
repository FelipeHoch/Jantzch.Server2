using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.OrderReports.GetOrderReports;

public record OrderReportsQuery(string ClientId, OrderReportResourceParameters Parameters) : IRequest<IEnumerable<ExpandoObject>>;
