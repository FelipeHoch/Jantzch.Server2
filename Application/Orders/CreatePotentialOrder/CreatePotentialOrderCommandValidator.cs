using FluentValidation;

namespace Jantzch.Server2;

public class CreatePotentialOrderCommandValidator : AbstractValidator<CreatePotentialOrderCommand>
{
    public CreatePotentialOrderCommandValidator()
    {
        RuleFor(x => x.Observations).MaximumLength(500);
        RuleFor(x => x.Client).NotNull();
        RuleFor(x => x.Type).NotNull();
    }
}
