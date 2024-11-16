using MediatR;

namespace Jantzch.Server2.Application.Orders.GetOrderImages;

public record OrderImagesQuery(string OrderId) : IRequest<OrderImagesResponse>; 