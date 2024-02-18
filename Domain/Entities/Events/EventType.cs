using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Reflection.Metadata;

namespace Jantzch.Server2.Domain.Entities.Events;

public class EventType
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string HexColor { get; set; } = string.Empty;
}
