using FluentValidation;

namespace Jantzch.Server2.Features.Materials.EditMaterial;

public class EditMaterialCommandValidator
{
    public class CommandValidator : AbstractValidator<EditMaterialCommand.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Model.Eu).NotNull().NotEmpty();
            RuleFor(x => x.Model.Name).NotNull().NotEmpty();
            RuleFor(x => x.Model.GroupId).NotNull().NotEmpty();
            RuleFor(x => x.Model.Value).NotNull().NotEmpty();            
        }
    }
}
