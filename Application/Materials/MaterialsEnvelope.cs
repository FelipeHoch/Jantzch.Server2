using Jantzch.Server2.Domain.Entities.Materials;

namespace Jantzch.Server2.Features.Materials;

public class MaterialsEnvelope
{
    public List<Material> Materials { get; set; } = [];

    public int MaterialsCount { get; set; }
}
