using FluentValidation;

namespace Jantzch.Server2.Application.Clients.CreateClient;

public class CreateClientCommandValidator
{
    public class ClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public ClientCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Cnpj).MaximumLength(14);
            RuleFor(x => x.Cpf).MaximumLength(11);
            RuleFor(x => x.Address).NotNull();
        }
    }   
}
