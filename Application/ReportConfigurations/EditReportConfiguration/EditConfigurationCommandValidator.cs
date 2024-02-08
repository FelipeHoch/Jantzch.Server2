using FluentValidation;

namespace Jantzch.Server2.Application.ReportConfigurations.EditConfiguration;

public class EditConfigurationCommandValidator
{
    public class CommandValidator : AbstractValidator<EditConfigurationCommand.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Model.BottomTitle).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Model.BottomText).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Model.PhoneContact).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Model.EmailContact).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Model.SiteUrl).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Model.MinValue).NotEmpty();
        }
    }
}
