﻿using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Services.Storage;
using Jantzch.Server2.Domain.Entities.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Clients.Deals;

[BsonIgnoreExtraElements]
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

    public List<string> OrderIds { get; private set; } = [];

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

    public bool? SolarEdge { get; set; } = false;
    
    public List<Image> Images { get; set; } = [];

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

    public void AddImage(Image image)
    {
        Images.Add(image);
    }

    public void AddImages(List<Image> images)
    {
        Images.AddRange(images);
    }

    public IEnumerable<Image>GetImages()
    {
        return Images;
    }

    public IEnumerable<Image> GetImages(ImageKeyEnum key)
    {
        return Images.Where(i => i.Key == key);
    }

    public void AddOrder(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (!OrderIds.Contains(order.Id))
        {
            OrderIds.Add(order.Id);
            LastUpdateAt = DateTime.Now;
        }
    }

    public void RemoveOrder(string orderId)
    {
        if (OrderIds.Contains(orderId))
        {
            OrderIds.Remove(orderId);
            LastUpdateAt = DateTime.Now;
        }
    }

    public bool HasOrder(string orderId)
    {
        return OrderIds.Contains(orderId);
    }
}

public class Image
{
    public string? Id { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; } = null;

    [BsonRepresentation(BsonType.String)]
    public ImageKeyEnum? Key
    {
        get; set;
    }

    public static Image Create(string id, string url, string description, ImageKeyEnum key)
    {
        return new Image
        {
            Id = id,
            Url = url,
            Description = description,
            Key = key
        };
    }
}

public class SystemPayment
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public double? Value { get; set; }
    public string? Text { get; set; }
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