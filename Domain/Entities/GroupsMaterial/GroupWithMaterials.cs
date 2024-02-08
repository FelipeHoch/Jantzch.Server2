using Jantzch.Server2.Domain.Entities.Materials;

namespace Jantzch.Server2.Domain.Entities.GroupsMaterial;

public class GroupWithMaterials : GroupMaterial
{
    public List<Material> Materials { get; set; } = [];
}
