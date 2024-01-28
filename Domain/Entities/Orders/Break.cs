using Jantzch.Server2.Domain.Entities.Users;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Jantzch.Server2.Domain.Entities.Orders;

public class Break
{
    [BsonElement("breakNumber")]
    public int BreakNumber { get; set; } = 0;

    [BsonElement("pausedBy")]
    public UserSimple? PausedBy { get; set; }

    [BsonElement("startDate")]
    public DateTime StartDate { get; set; }

    [BsonElement("endDate")]
    [BsonIgnoreIfNull]
    public DateTime? EndDate { get; set; }

    [BsonElement("descriptive")]
    [BsonIgnoreIfNull]
    [MaxLength(400)]
    public string? Descriptive { get; set; }
}
