using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Taxes;
using System.Text.Json.Serialization;

namespace Jantzch.Server2.Application.OrderReports;

public class OrderReportResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Id { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ReportNumber { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ClientSimple? Client { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<OrderExport>? Orders { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GeneratedBy { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? GeneratedAt { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? DueDate { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? TotalValue { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Tax>? Taxes { get; set; } = default;
}
