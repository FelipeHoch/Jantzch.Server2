using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jantzch.Server2.Domain;

public class Material
{
    [JsonIgnore]
    [BsonId]
    public ObjectId? Id { get; set; }

    public string Name { get; set; }

    public double Value { get; set; }

    public string Eu { get; set; }

    public string CreatedBy { get; set; }

    [JsonIgnore]
    public ObjectId? GroupIdObject { get; set; }

    [NotMapped]
    [JsonPropertyName("id")]
    public string MongoId => Id?.ToString() ?? string.Empty;

    [NotMapped]
    [JsonPropertyName("groupId")]
    public string MongoGroupId => GroupIdObject?.ToString() ?? null;
}
