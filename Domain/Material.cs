using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain;

public class Material
{
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("value")]
    public double Value { get; set; }

    [BsonElement("eu")]
    public string Eu { get; set; }

    [BsonElement("createdBy")]
    public string CreatedBy { get; set; }

    [BsonElement("groupId")]
    public ObjectId? GroupId { get; set; }
}
