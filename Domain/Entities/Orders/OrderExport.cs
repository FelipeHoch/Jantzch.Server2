using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Jantzch.Server2.Domain.Entities.Materials;

namespace Jantzch.Server2.Domain.Entities.Orders;

public class OrderExport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("orderNumber")]
    public int? OrderNumber { get; set; }

    [BsonElement("startDate")]
    public DateTime? StartDate { get; set; }

    [BsonElement("finishedAt")]
    public DateTime? FinishedAt { get; set; }

    [BsonElement("descriptive")]
    public string Descriptive { get; set; }

    [BsonElement("manPower")]
    public double ManPower { get; set; }

    [BsonElement("materialCust")]
    [BsonIgnoreIfNull]
    public double MaterialCust { get; set; }

    [BsonElement("materials")]
    [BsonIgnoreIfNull]
    public List<MaterialHistory>? Materials { get; set; }
}
