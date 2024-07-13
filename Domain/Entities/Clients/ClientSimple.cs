using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Clients;

[BsonIgnoreExtraElements]
public class ClientSimple
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    public Address? Address { get; set; }

    public Location? Location { get; set; }

    public Route? Route { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }
}
