using FluentValidation;

namespace Jantzch.Server2.Application.ReportConfigurations.CreateConfiguration;

public class CreateConfigurationCommandValidator
{
    public class ConfigurationCommandValidator : AbstractValidator<CreateConfigurationCommand>
    {
        public ConfigurationCommandValidator()
        {
            RuleFor(x => x.ReportKey).NotEmpty().MaximumLength(100);
            RuleFor(x => x.BottomTitle).NotEmpty().MaximumLength(100);
            RuleFor(x => x.BottomText).NotEmpty().MaximumLength(500);
            RuleFor(x => x.PhoneContact).NotEmpty().MaximumLength(100);
            RuleFor(x => x.EmailContact).NotEmpty().MaximumLength(100);
            RuleFor(x => x.SiteUrl).NotEmpty().MaximumLength(100);
        }
    }   
}
