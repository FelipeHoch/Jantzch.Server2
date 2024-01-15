using FluentValidation;
using Jantzch.Server2.Domain.Entities.Materials;
using MediatR;

namespace Jantzch.Server2.Features.Materials.EditMaterial;

public class EditMaterialCommand
{
    public string Name { get; set; }

    public double Value { get; set; }

    public string Eu { get; set; }

    public string? GroupId { get; set; }

    public record Command(EditMaterialCommand Model, string Id) : IRequest<Material>;
}
