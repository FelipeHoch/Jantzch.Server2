using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Jantzch.Server2.Infrastructure.Google.Models;

public sealed class Distance
{
    [JsonPropertyName("destination_addresses")]
    public List<string> DestinationAddresses { get; set; }

    [JsonPropertyName("origin_addresses")]
    public List<string> OriginAddresses { get; set; }

    public List<Row> Rows { get; set; }

    public string Status { get; set; }
}

public sealed class Measurement
{
    [BsonElement("Text")]
    public string Text { get; set; }

    [BsonElement("Value")]
    public int Value { get; set; }
}

public sealed class Element
{
    public Measurement Distance { get; set; }

    public Measurement Duration { get; set; }

    public string Status { get; set; }
}

public sealed class Row
{
    public List<Element> Elements { get; set; }
}