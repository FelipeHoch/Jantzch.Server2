using Newtonsoft.Json;

namespace Jantzch.Server2.Application.Events;

public class EventTypeResponse
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Id { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Name { get; set; } = default;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? HexColor { get; set; } = default;
}
