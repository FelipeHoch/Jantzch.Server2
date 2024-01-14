using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Jantzch.Server2.Domain;

public class GroupMaterial
{
    [JsonIgnore]
    public ObjectId Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    [JsonPropertyName("id")]
    public string MongoId => Id.ToString();
}
