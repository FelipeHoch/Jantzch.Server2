using FluentValidation;

namespace Jantzch.Server2.Application.OrderReports.CreateOrderReport;

public class CreateOrderReportCommandValidator : AbstractValidator<CreateOrderReportCommand>
{
    public CreateOrderReportCommandValidator()
    {
        RuleFor(x => x.OrdersId).NotEmpty();
    }
}
