using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using MediatR;

namespace Jantzch.Server2.Application.GroupsMaterial.EditGroupMaterial;

public class EditGroupMaterialCommand
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public record Command(EditGroupMaterialCommand Model, string Id) : IRequest<GroupMaterial>;
}
