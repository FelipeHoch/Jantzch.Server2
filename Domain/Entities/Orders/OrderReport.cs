using Jantzch.Server2.Domain.Entities.Clients;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Jantzch.Server2.Domain.Entities.Taxes;

namespace Jantzch.Server2.Domain.Entities.Orders;

public class OrderReport
{
    public OrderReport(Client client, int reportNumber, string generatedBy, List<OrderExport> orderExports, List<Tax> taxes)
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

    public Client Client { get; set; }

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
