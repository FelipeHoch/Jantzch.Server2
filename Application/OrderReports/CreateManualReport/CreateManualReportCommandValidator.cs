using FluentValidation;

namespace Jantzch.Server2.Application.OrderReports.CreateManualReport;

public class CreateManualReportCommandValidator : AbstractValidator<CreateManualReportCommand>
{
    public CreateManualReportCommandValidator()
    {
        RuleFor(x => x.MaterialsUsed).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
    }
}
