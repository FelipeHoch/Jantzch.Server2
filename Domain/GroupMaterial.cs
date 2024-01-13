using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain;

public class GroupMaterial
{
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }


    [BsonElement("description")]
    public string Description { get; set; }

}
