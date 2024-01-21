using FluentValidation;

namespace Jantzch.Server2.Application.Taxes.EditTax;

public class EditTaxCommandValidator
{
    public class CommandValidator : AbstractValidator<EditTaxCommand.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(100).WithMessage("Name is required");
            RuleFor(x => x.Model.Type).NotEmpty().MaximumLength(100).WithMessage("Type is required");
            RuleFor(x => x.Model.Value).NotEmpty().WithMessage("Value is required");
        }
    }
}
