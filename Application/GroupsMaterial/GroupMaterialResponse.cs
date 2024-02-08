using Jantzch.Server2.Domain.Entities.Materials;
using System.Text.Json.Serialization;

namespace Jantzch.Server2.Application.GroupsMaterial;

public class GroupMaterialResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Id { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Material>? Materials { get; set; }
}
