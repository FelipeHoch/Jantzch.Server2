using System.Text.Json.Serialization;
using Domain.Entities.Orders.Enums;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Users;

namespace Jantzch.Server2;

public class PotentialOrderResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Id { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PotentialOrderStatus Status { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UserSimple? CreatedBy { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? CreatedAt { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? ConvertedToOrderAt { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? EstimatedCompletionTimeInMilliseconds { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ClientSimple? Client { get; set; } = default;

    public OrderType Type { get; set; } = OrderType.Eletric;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Observations { get; set; } = default;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ConvertedOrderId { get; set; } = default;
}
