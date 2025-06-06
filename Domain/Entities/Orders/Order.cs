﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Materials;
using Domain.Entities.Orders.Enums;

namespace Jantzch.Server2.Domain.Entities.Orders;

public class Order
{
    public Order()
    {
        if (string.IsNullOrEmpty(Id)) return;

        SetStatusOnCreation();
        SetStartDateOnOpen();
    }

    public Order(ClientSimple client, int predictedTime, string? observations, UserSimple createdBy, int orderNumber, OrderType type)
    {
        OrderNumber = orderNumber;
        Client = client;
        PredictedTime = predictedTime;
        Observations = observations;
        CreatedBy = createdBy;
        Status = "scheduled";
        ScheduledDate = DateTime.UtcNow;
        Type = type;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public int OrderNumber { get; set; } = 0;

    public UserSimple? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("finishedAt")]
    [BsonIgnoreIfNull]
    public DateTime? FinishedAt { get; set; }

    public UserSimple? FinishedBy { get; set; }

    [BsonIgnoreIfNull]
    [MaxLength(400)]
    public string? Descriptive { get; set; }

    [MaxLength(50)]
    public string Status { get; set; }

    public ClientSimple Client { get; set; }

    public int PredictedTime { get; set; } = 0;

    public OrderType Type { get; set; } = OrderType.Eletric;

    [BsonIgnoreIfNull]
    [MaxLength(400)]
    public string? Observations { get; set; }

    [BsonIgnoreIfNull]
    public DateTime? StartDate { get; set; }

    [BsonIgnoreIfNull]
    public DateTime? ScheduledDate { get; set; }

    [BsonIgnoreIfNull]
    public List<MaterialHistory>? MaterialsUsed { get; set; } = [];

    public List<UserSimple> Workers { get; set; } = [];

    [BsonIgnoreIfNull]
    public bool? IsReported { get; set; } = false;

    [BsonIgnoreIfNull]
    public List<Break> BreaksHistory { get; set; } = [];

    [BsonIgnoreIfNull]
    public double? HoursWorked { get; set; } = 0;

    [BsonIgnoreIfNull]
    public double? ManualManPower { get; set; } = null;

    [BsonIgnoreIfNull]
    public double?  ExpectedValue { get; set; } = 0;

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? DealId { get; private set; }

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

        if (HoursWorked < 1) HoursWorked = 1;
    }

    public void SetStatusOnCreation()
    {
        Status = ScheduledDate != null ? "scheduled" : "open";
    }

    private void SetStartDateOnOpen()
    {
        if (Status == "open") StartDate = DateTime.UtcNow;
    }

    public void AssignToDeal(string dealId)
    {
        if (string.IsNullOrEmpty(dealId))
            throw new ArgumentNullException(nameof(dealId));

        DealId = dealId;
    }

    public void RemoveFromDeal()
    {
        DealId = null;
    }
}