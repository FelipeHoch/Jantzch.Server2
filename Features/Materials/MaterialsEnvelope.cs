using Jantzch.Server2.Domain;

namespace Jantzch.Server2.Features.Materials;

public class MaterialsEnvelope
{
    public List<Material> Materials { get; set; } = new();

    public int MaterialsCount { get; set; }
}
