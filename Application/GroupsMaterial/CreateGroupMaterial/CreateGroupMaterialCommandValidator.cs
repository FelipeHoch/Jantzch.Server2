using FluentValidation;

namespace Jantzch.Server2.Application.GroupsMaterial.CreateGroupMaterial;

public class CreateGroupMaterialCommandValidator
{
    public class GroupMaterialCommandValidator : AbstractValidator<CreateGroupMaterialCommand>
    {
        public GroupMaterialCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }   
}
