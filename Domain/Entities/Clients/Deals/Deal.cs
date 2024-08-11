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

    public ProjectStatusEnum? ProjectStatus { get; set; } = ProjectStatusEnum.GroupingPhotos;

    public ClientSimple Client { get; set; }

    public List<string> OrderIds { get; set; } = [];

    public string? IntegrationId { get; set; }

    public string? Material { get; set; }

    public string? PanelInstallation { get; set; }

    public string? InversorLocalization { get; set; }

    public string? CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? LastUpdateAt { get; set; }

    public DateTime? DealConfirmedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public List<HistoryStatus> HistoryStatus { get; set; } = [];

    public UserSimple? SoldedBy { get; set; }

    public PaymentStatusEnum? PaymentStatus { get; set; }

    public SystemPayment? SystemPayment { get; set; }

    public Commission? Commission { get; set; }

    public string? AppAccess { get; set; }

    public string? Datalogger { get; set; }

    public string? LinkForImages { get; set; }

    public string? Order { get; set; }

    public void NextStatus(StatusEnum status, UserSimple user, DateTime? date = null)
    {              
        Status = status;

        HistoryStatus historyStatus = new()
        {
            Status = status,
            Date = (DateTime)(date == null ? DateTime.Now : date),
            User = user
        };

        HistoryStatus.Add(historyStatus);

        LastUpdateAt = (DateTime)(date == null ? DateTime.Now : date);

        if (status == StatusEnum.InstallationCompleted)
        {
            ClosedAt = (DateTime)(date == null ? DateTime.Now : date);
        }
        else
        {
            ClosedAt = null;        
        }
    }
}

public class SystemPayment
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public double Value { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
}

public class Commission
{
    public double Value { get; set; }
    public CommissionStatusEnum Status { get; set; }
}


public class HistoryStatus
{
    public UserSimple User { get; set; }

    public StatusEnum Status { get; set; } = StatusEnum.PendingMaterial;

    public DateTime Date { get; set; } = DateTime.UtcNow;
}