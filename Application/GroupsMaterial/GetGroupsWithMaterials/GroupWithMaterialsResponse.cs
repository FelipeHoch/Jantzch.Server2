using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Domain.Entities.Materials;

namespace Jantzch.Server2.Application.GroupsMaterial.GetGroupsWithMaterials;

public class GroupWithMaterialsResponse : GroupMaterial
{
    public List<Material> Materials { get; set; } = [];
}
