using FluentValidation;

namespace Jantzch.Server2;

public class EditPotentialOrderCommandValidator : AbstractValidator<EditPotentialOrderCommand.Command>
{
    public EditPotentialOrderCommandValidator()
    {
        RuleFor(x => x.Model.Client).NotNull();
        RuleFor(x => x.Model.EstimatedCompletionTimeInMilliseconds).NotNull();
        RuleFor(x => x.Model.Type).NotNull();
    }
}
