using System.ComponentModel.DataAnnotations;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2;

public class PotentialOrder
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }
    
    public PotentialOrderStatus Status { get; set; } = PotentialOrderStatus.Pending;

    public UserSimple CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonIgnoreIfNull]
    public DateTime? ConvertedToOrderAt { get; set; }

    [BsonIgnoreIfNull]
    public int EstimatedCompletionTimeInMilliseconds { get; set; } = 0;

    public ClientSimple Client { get; set; }

    [BsonIgnoreIfNull]
    [MaxLength(400)]
    public string? Observations { get; set; }

    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ConvertedOrderId  { get; set; }

    public Order ConvertToOrder(UserSimple convertedBy, int orderNumber)
    {
        Status = PotentialOrderStatus.Converted;

        ConvertedToOrderAt = DateTime.UtcNow;

        var estimatedCompletionTimeInMinutes = EstimatedCompletionTimeInMilliseconds / 60000;

        return new Order(Client, estimatedCompletionTimeInMinutes, Observations, convertedBy, orderNumber);
    }

    public void RelateOrder(string orderId)
    {
        ConvertedOrderId = orderId;
    }

    public void RelateUser(UserSimple user)
    {
        CreatedBy = user;
    }

    public void Cancel()
    {
        Status = PotentialOrderStatus.Cancelled;
    }

    public bool IsCancelled()
    {
        return Status == PotentialOrderStatus.Cancelled;
    }
}
