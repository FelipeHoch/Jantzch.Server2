using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Orders.GetOrders;

public class GetOrdersHandler : IRequestHandler<OrdersQuery, IEnumerable<ExpandoObject>>
{
    private readonly IOrderRepository _orderRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetOrdersHandler(
        IOrderRepository orderRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(OrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(orders, _httpContextAccessor.HttpContext.Response);

        var ordersList = orders.ToList();

        var ordersResponse = _mapper.Map<List<OrderResponse>>(ordersList);

        var ordersShaped = _dataShapingService.ShapeDataList(ordersResponse, request.Parameters.Fields);

        return ordersShaped;
    }
}
