using FluentValidation;

namespace Jantzch.Server2.Application.Clients.EditAddress;

public class EditAddressCommandValidator
{
    public class CommandValidator : AbstractValidator<EditAddressCommand.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Model.Street).NotEmpty().MaximumLength(400);
            RuleFor(x => x.Model.StreetNumber).NotEmpty();
            RuleFor(x => x.Model.District).NotEmpty().MaximumLength(400);
            RuleFor(x => x.Model.City).NotEmpty().MaximumLength(400);
            RuleFor(x => x.Model.State).NotEmpty().MaximumLength(400);
        }
    }
}
