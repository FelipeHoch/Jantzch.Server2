using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Clients;

public class ClientSimple
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    public Address? Address { get; set; }

    public Location? Location { get; set; }
}
