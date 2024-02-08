using Jantzch.Server2.Domain.Entities.Materials;
using MediatR;

namespace Jantzch.Server2.Application.OrderReports.CreateManualReport;

public class CreateManualReportCommand
{
    public List<MaterialHistory> MaterialsUsed { get; set; } = [];

    public DateTime? StartDate { get; set; }

    public DateTime? FinishedAt { get; set; }

    public string Descriptive { get; set; } = string.Empty;

    public double Value { get; set; }

    public record Command(
        List<CreateManualReportCommand> ManualReports,
        string ClientId,
        string? TaxesId
    ) : IRequest<OrderReportResponse>;
}
