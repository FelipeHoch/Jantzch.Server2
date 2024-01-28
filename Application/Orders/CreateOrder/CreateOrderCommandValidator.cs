using FluentValidation;

namespace Jantzch.Server2.Application.Orders.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Observations).MaximumLength(500);
        RuleFor(x => x.Client).NotNull();
        RuleFor(x => x.Workers).NotEmpty();
    }
}
