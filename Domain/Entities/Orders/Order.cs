using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Materials;

namespace Jantzch.Server2.Domain.Entities.Orders;

public class Order
{
    public Order()
    {
        SetStatusOnCreation();
        SetStartDateOnOpen();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; } = string.Empty;

    [BsonElement("orderNumber")]
    public int OrderNumber { get; set; } = 0;

    [BsonElement("createdBy")]
    public UserSimple? CreatedBy { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [BsonElement("finishedAt")]
    [BsonIgnoreIfNull]
    public DateTime? FinishedAt { get; set; }

    [BsonElement("finishedBy")]
    public UserSimple? FinishedBy { get; set; }

    [BsonElement("descriptive")]
    [BsonIgnoreIfNull]
    [MaxLength(400)]
    public string? Descriptive { get; set; }

    [BsonElement("status")]
    [MaxLength(50)]
    public string Status { get; set; }

    [BsonElement("client")]
    public ClientSimple Client { get; set; }

    [BsonElement("predictedTime")]
    public int PredictedTime { get; set; } = 0;

    [BsonElement("observations")]
    [BsonIgnoreIfNull]
    [MaxLength(400)]
    public string? Observations { get; set; }

    [BsonElement("startDate")]
    [BsonIgnoreIfNull]
    public DateTime? StartDate { get; set; }

    [BsonElement("scheduledDate")]
    [BsonIgnoreIfNull]
    public DateTime? ScheduledDate { get; set; }

    [BsonElement("materialsUsed")]
    [BsonIgnoreIfNull]
    public List<MaterialHistory>? MaterialsUsed { get; set; }

    [BsonElement("workers")]
    public List<UserSimple> Workers { get; set; } = [];

    [BsonElement("isReported")]
    [BsonIgnoreIfNull]
    public bool? IsReported { get; set; }

    [BsonElement("breaksHistory")]
    [BsonIgnoreIfNull]
    public List<Break> BreaksHistory { get; set; } = [];

    [BsonElement("hoursWorked")]
    [BsonIgnoreIfNull]
    public double? HoursWorked { get; set; } = 0;

    public void FinishOrder(string userId, string userName)
    {
        if (FinishedAt is null)
        {
            IsReported = false;

            FinishedAt = DateTime.UtcNow;

            FinishedBy = new UserSimple
            {
                Id = userId,
                Name = userName
            };
        }

        CalculateHoursWorked();
    }

    public void PauseOrder(string descriptive, string userId, string userName)
    {
        if (Status == "open")
        {
            Status = "paused";

            BreaksHistory.Add(new Break
            {
                StartDate = DateTime.UtcNow,
                Descriptive = descriptive,
                BreakNumber = BreaksHistory.Count,
                PausedBy = new UserSimple
                {
                    Id = userId,
                    Name = userName
                }
            });
        }
    }

    public void CalculateHoursWorked()
    {
        var timeDiff = FinishedAt - StartDate;

        var minutesWorked = timeDiff?.TotalMinutes;

        if (BreaksHistory.Count > 0)
        {

            var minutesStopped = BreaksHistory.Sum(x => (x.EndDate - x.StartDate).Value.TotalMinutes);

            minutesWorked -= minutesStopped;
        }

        HoursWorked = minutesWorked / 60;
    }

    private void SetStatusOnCreation()
    {
        Status = ScheduledDate != null ? "scheduled" : "open";
    }

    private void SetStartDateOnOpen()
    {
        if (Status == "open") StartDate = DateTime.Now;
    }
}