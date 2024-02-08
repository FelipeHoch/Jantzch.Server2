using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Jantzch.Server2.Domain.Entities.Materials;

public class Material
{
    [JsonIgnore]
    [BsonId]
    public ObjectId? Id { get; set; } = ObjectId.GenerateNewId();

    public string Name { get; set; }

    public double Value { get; set; }

    public string Eu { get; set; }

    public string CreatedBy { get; set; }

    [JsonIgnore]
    [BsonElement("groupId")]
    public ObjectId? GroupMaterialId { get; set; }

    [NotMapped]
    [JsonProperty("id")]
    public string MongoId => Id?.ToString() ?? string.Empty;

    [NotMapped]
    [BsonIgnore]
    [BsonElement("groupId")]
    [JsonProperty("groupId")]
    public string MongoGroupId => GroupMaterialId?.ToString() ?? null;
}
