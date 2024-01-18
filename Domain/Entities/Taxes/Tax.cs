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
}
