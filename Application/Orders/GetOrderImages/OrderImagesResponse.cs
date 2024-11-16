using Jantzch.Server2.Domain.Entities.Clients.Deals;

namespace Jantzch.Server2.Application.Orders.GetOrderImages;

public class OrderImagesResponse
{
    public string OrderId { get; set; } = string.Empty;
    public List<Image> Images { get; set; } = [];
} 