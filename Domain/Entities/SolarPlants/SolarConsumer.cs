using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.SolarPlants;

public class SolarConsumer
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Consumption { get; set; } = 0;

    public bool IsActived { get; set; } = true;

    public string? CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastUpdateAt { get; set; }
}
