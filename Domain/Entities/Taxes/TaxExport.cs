using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Taxes;

public class TaxExport
{
    [BsonElement("Type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("Value")]
    public double Value { get; set; } = 0;

    [BsonElement("TotalValue")]
    public double TotalValue { get; set; } = 0;

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Code")]
    public int? Code { get; set; }
}
