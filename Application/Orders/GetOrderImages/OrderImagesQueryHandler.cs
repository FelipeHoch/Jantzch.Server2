using Jantzch.Server2.Domain.Entities.Clients.Deals;
using MediatR;

namespace Jantzch.Server2.Application.Orders.GetOrderImages;

public class OrderImagesQueryHandler : IRequestHandler<OrderImagesQuery, OrderImagesResponse>
{
    private readonly IDealRepository _dealRepository;

    public OrderImagesQueryHandler(IDealRepository dealRepository)
    {
        _dealRepository = dealRepository;
    }

    public async Task<OrderImagesResponse> Handle(OrderImagesQuery request, CancellationToken cancellationToken)
    {
        var deal = await _dealRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);

        if (deal == null)
            return new OrderImagesResponse 
            { 
                OrderId = request.OrderId,
                Images = []
            };

        return new OrderImagesResponse
        {
            OrderId = request.OrderId,
            Images = deal.Images
        };
    }
} 