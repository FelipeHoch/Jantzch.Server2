using Jantzch.Server2.Domain.Entities.Clients;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Jantzch.Server2.Domain.Entities.Taxes;

namespace Jantzch.Server2.Domain.Entities.Orders;

public class OrderReport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }

    public int ReportNumber { get; set; }

    public Client Client { get; set; }

    public List<OrderExport> Orders { get; set; } = [];

    public string GeneratedBy { get; set; }

    public DateTime GeneratedAt { get; set; }

    public DateTime? DueDate { get; set; }

    public double TotalValue { get; set; }

    public List<TaxExport> Taxes { get; set; } = [];
}
