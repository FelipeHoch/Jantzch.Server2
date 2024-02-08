using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Orders;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Materials;

namespace Jantzch.Server2.Application.OrderReports.Models;

[BsonIgnoreExtraElements]
public class DetailedOrderForExport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; } = string.Empty;

    public int OrderNumber { get; set; } = 0;

    public UserSimple CreatedBy { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FinishedAt { get; set; }

    [BsonIgnoreIfNull]
    public string Descriptive { get; set; } = string.Empty;

    public Client Client { get; set; }

    [BsonIgnoreIfNull]
    public List<MaterialHistory> MaterialsUsed { get; set; } = [];

    [BsonIgnoreIfNull]
    public List<User> Workers { get; set; } = [];

    [BsonIgnoreIfNull]
    public List<Break> BreaksHistory { get; set; } = [];

    [BsonIgnoreIfNull]
    public double? HoursWorked { get; set; }
}
