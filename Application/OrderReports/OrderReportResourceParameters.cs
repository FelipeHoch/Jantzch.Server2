using Jantzch.Server2.Application.Shared;

namespace Jantzch.Server2.Application.OrderReports;

public class OrderReportResourceParameters : ResourceParameters
{
    public string? Client { get; set; }
    public string? GeneratedBy { get; set; }
    public new string OrderBy { get; set; } = "reportNumber";
}
