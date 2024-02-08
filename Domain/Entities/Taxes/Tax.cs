using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Orders;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Jantzch.Server2.Domain.Entities.Taxes;

public class Tax
{
    [JsonIgnore]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    public string Name { get; set; }

    public string Type { get; set; }

    public string CreatedBy { get; set; }

    public double Value { get; set; }

    public int Code { get; set; }

    [NotMapped]
    [JsonPropertyName("id")]
    public string MongoId => Id.ToString();

    [NotMapped]
    [JsonPropertyName("totalValue")]
    public double TotalValue { get; set; }

    public double ApplyTax(double value, int distance)
    {
        if (Type == "percent")
        {
            return value * (Value / 100);
        }
        else if (Type == "amount")
        {
            return Value;
        }
        else
        {
            var distanceInKm = distance / 1000;

            return distanceInKm * Value;
        }
    }

}
