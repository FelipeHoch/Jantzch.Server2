using Jantzch.Server2.Application.OrderReports.Models;
using Jantzch.Server2.Domain.Entities.Materials;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Orders;

public class OrderExport
{
    public OrderExport()
    {
    }

    public OrderExport(DetailedOrderForExport detailedOrderForExport, double minValue)
    {
        Id = detailedOrderForExport.Id;
        OrderNumber = detailedOrderForExport.OrderNumber;
        StartDate = detailedOrderForExport.StartDate;
        FinishedAt = detailedOrderForExport.FinishedAt;
        Descriptive = detailedOrderForExport.Descriptive;
        Materials = detailedOrderForExport.MaterialsUsed;

        CalculateTotalManPower(detailedOrderForExport, minValue);
        CalculateTotalMaterialCost();           
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    public int? OrderNumber { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FinishedAt { get; set; }

    public string Descriptive { get; set; }

    public double ManPower { get; set; }

    [BsonIgnoreIfNull]
    public double MaterialCust { get; set; } = 0.0;

    [BsonIgnoreIfNull]
    public List<MaterialHistory> Materials { get; set; } = [];

    public void CalculateTotalManPower(DetailedOrderForExport detailedOrderForExport, double minValue)
    {
        var manPower = 0.0;

        if (detailedOrderForExport.HasManualManPower())
        {
            ManPower = (double)detailedOrderForExport.ManualManPower;

            return;
        }

        detailedOrderForExport.Workers.ForEach(worker =>
        {
            var hoursWorked = 0.0;

            if (detailedOrderForExport.HoursWorked is not null)
            {
                hoursWorked = (double)detailedOrderForExport.HoursWorked;
            }

            manPower += worker.CalculateTotalManPower((double)detailedOrderForExport.HoursWorked);
        });

        ManPower = manPower;

        SetMinManPower(minValue);
    }

    public void CalculateTotalMaterialCost()
    {
        var materialCost = 0.0;

        Materials.ForEach(material =>
        {
            materialCost += material.CalculateTotalCost();
        });

        MaterialCust = materialCost;
    }

    public void SetMinManPower(double minValue)
    {
        if (ManPower < minValue)
        {
            ManPower = minValue;
        }
    }
}
