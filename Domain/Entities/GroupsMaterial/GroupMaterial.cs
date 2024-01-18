using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jantzch.Server2.Domain.Entities.GroupsMaterial;

public class GroupMaterial
{
    [JsonIgnore]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    public string Name { get; set; }

    public string? Description { get; set; }

    [JsonPropertyName("id")]
    [NotMapped]
    public string MongoId => Id.ToString();
}
