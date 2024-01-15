using Jantzch.Server2.Domain.Entities.Materials;
using MediatR;
using System.Text.Json.Serialization;
using static Jantzch.Server2.Application.Materials.CreateMaterial.CreateMaterialCommand;

namespace Jantzch.Server2.Application.Materials.CreateMaterial;

public class CreateMaterialCommand : IRequest<Material>
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }

    [JsonPropertyName("eu")]
    public string Eu { get; set; }

    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; }

    [JsonPropertyName("groupId")]
    public string? GroupId { get; set; }

}
