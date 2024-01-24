using FluentValidation;

namespace Jantzch.Server2.Application.Clients.EditClient;

public class EditClientCommandValidator
{
    public class CommandValidator : AbstractValidator<EditClientCommand.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Model.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Model.Email).EmailAddress();
            RuleFor(x => x.Model.PhoneNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Model.Cnpj).MaximumLength(14);
            RuleFor(x => x.Model.Cpf).MaximumLength(11);
        }
    }
}
