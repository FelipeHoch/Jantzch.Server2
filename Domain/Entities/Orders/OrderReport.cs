using Jantzch.Server2.Domain.Entities.Clients;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Infraestructure.Errors;
using System.Net;
using Jantzch.Server2.Domain.Entities.Clients.Constants;

namespace Jantzch.Server2.Domain.Entities.Orders;

[BsonIgnoreExtraElements]
public class OrderReport
{
    public OrderReport(ClientSimple client, int reportNumber, string generatedBy, List<OrderExport> orderExports, List<Tax> taxes)
    {
        Client = client;
        ReportNumber = reportNumber;
        GeneratedBy = generatedBy;
        Orders = orderExports;
        Taxes = taxes;

        CalculateTotalValue();

        AddTaxValueToTotalValue(taxes, client.Route.Distance.Value);

        SetDueDate();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    public int ReportNumber { get; set; }

    public ClientSimple Client { get; set; }

    public List<OrderExport> Orders { get; set; } = [];

    public string GeneratedBy { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; private set; }

    public double TotalValue { get; set; } = 0;

    public List<Tax> Taxes { get; set; } = [];

    public void CalculateTotalValue()
    {
        var totalValue = 0.0;

        Orders.ForEach(order =>
        {
            totalValue += order.ManPower + order.MaterialCust;
        });

        TotalValue = totalValue;
    }

    public void SetDueDate()
    {
        DueDate = Orders.Max(order => order.FinishedAt);
    }

    public void AddTaxValueToTotalValue(List<Tax> taxes, int distance)
    {   
        var oldTotalValue = TotalValue;

        taxes.ForEach(tax =>
        {
            var taxTotalValue = tax.ApplyTax(oldTotalValue, distance);

            TotalValue += taxTotalValue;

            tax.TotalValue = taxTotalValue;
        });
    }
}
