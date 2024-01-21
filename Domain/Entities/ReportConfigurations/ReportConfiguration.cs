using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jantzch.Server2.Domain.Entities.ReportConfigurations;

public class ReportConfiguration
{
    [JsonIgnore]
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    public string ReportKey { get; set; }

    public string BottomTitle { get; set; }

    public string BottomText { get; set; }

    public string PhoneContact { get; set; }

    public string EmailContact { get; set; }

    public string SiteUrl { get; set; }

    [NotMapped]
    [JsonPropertyName("id")]
    public string MongoId => Id.ToString();
}
