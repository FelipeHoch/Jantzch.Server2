using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using MediatR;

namespace Jantzch.Server2.Application.GroupsMaterial.CreateGroupMaterial;

public class CreateGroupMaterialCommand : IRequest<GroupMaterial>
{
    public string Name { get; set; }

    public string? Description { get; set; }
}
