using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Materials;

[BsonNoId]
public class MaterialHistory
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("id")]
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; } = 0;

    [BsonIgnoreIfNull]
    public double Value { get; set; } = 0;

    public double CalculateTotalCost()
    {
        return Quantity * Value;
    }
}
