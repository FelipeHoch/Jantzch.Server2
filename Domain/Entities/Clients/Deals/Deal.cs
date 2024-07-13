using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;
using Jantzch.Server2.Domain.Entities.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Clients.Deals;

public class Deal
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Type { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public double Value { get; set; } = 0;

    public string InstalationType { get; set; } = string.Empty;

    public string StructureType { get; set; } = string.Empty;

    public Address? Address { get; set; }

    public string Phase { get; set; } = string.Empty;

    public StatusEnum Status { get; set; } = StatusEnum.PendingMaterial;

    public ClientSimple Client { get; set; }

    public List<string> OrderIds { get; set; } = [];

    public string? IntegrationId { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? DealConfirmedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public List<HistoryStatus> HistoryStatus { get; set; } = [];
}


public class HistoryStatus
{
    public UserSimple User { get; set; }

    public StatusEnum Status { get; set; } = StatusEnum.PendingMaterial;

    public DateTime Date { get; set; } = DateTime.UtcNow;
}