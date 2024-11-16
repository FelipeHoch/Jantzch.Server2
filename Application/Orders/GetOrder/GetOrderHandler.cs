using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Orders.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Dynamic;
using System.Net;

namespace Jantzch.Server2.Application.Orders.GetOrder;

public class GetOrderHandler : IRequestHandler<OrderQuery, ExpandoObject>
{
    private readonly IOrderRepository _orderRepository;

    private readonly IMapper _mapper;

    private readonly IDataShapingService _dataShapingService;

    public GetOrderHandler(
        IOrderRepository orderRepository,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _dataShapingService = dataShapingService;
    }

    public async Task<ExpandoObject> Handle(OrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = OrdersErrorMessages.NOT_FOUND });
        }

        if (!string.IsNullOrEmpty(request.DealId) && order.DealId != request.DealId)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = "Order not found for the specified deal" });
        }

        var orderResponse = _mapper.Map<OrderResponse>(order);

        var orderShaped = _dataShapingService.ShapeData(orderResponse, request.Fields);

        return orderShaped;
    }
}
