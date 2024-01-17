using FluentValidation;

namespace Jantzch.Server2.Application.GroupsMaterial.EditGroupMaterial;

public class EditGroupMaterialCommandValidator
{
    public class CommandValidator : AbstractValidator<EditGroupMaterialCommand.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Model.Description).MaximumLength(500);
            RuleFor(x => x.Model.Name).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}
