using FluentValidation;

namespace Jantzch.Server2.Application.Taxes.CreateTax;

public class CreateTaxCommandValidator
{
    public class TaxCommandValidator : AbstractValidator<CreateTaxCommand>
    {
        public TaxCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Type).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Value).NotEmpty();
        }
    }   
}
