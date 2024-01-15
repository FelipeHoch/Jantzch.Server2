using FluentValidation;
using Jantzch.Server2.Application.Materials.DeleteMaterial;

namespace Jantzch.Server2.Features.Materials;

public class DeleteMaterialCommandValidator
{
    public class CommandValidator : AbstractValidator<DeleteMaterialCommand.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
