using FluentValidation;
using MongoDB.Bson;

namespace Jantzch.Server2.Application.Materials.CreateMaterial;

public class CreateMaterialCommandValidator
{
    public class MaterialDataValidator : AbstractValidator<CreateMaterialCommand>
    {
        public MaterialDataValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Value).NotNull().NotEmpty();
            RuleFor(x => x.Eu).NotNull().NotEmpty();
            RuleFor(x => x.CreatedBy).NotNull().NotEmpty();
            RuleFor(x => x.GroupId).NotNull().NotEmpty().Must(x => ObjectId.TryParse(x, out _));
        }
    }
}
