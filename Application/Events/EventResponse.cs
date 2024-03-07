using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Users;
using Newtonsoft.Json;

namespace Jantzch.Server2.Application.Events;

public class EventResponse
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Id { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Name { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? StartDate { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? EndDate { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public EventType? EventType { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public UserSimple? User { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public bool? NotifyUser { get; set; } = default;
}
