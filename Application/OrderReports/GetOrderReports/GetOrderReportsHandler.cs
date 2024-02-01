using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.OrderReports.GetOrderReports;

public class GetOrderReportsHandler : IRequestHandler<OrderReportsQuery, IEnumerable<ExpandoObject>>
{
    private readonly IOrderReportRepository _orderReportRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetOrderReportsHandler(
        IOrderReportRepository orderReportRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _orderReportRepository = orderReportRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(OrderReportsQuery request, CancellationToken cancellationToken)
    {
        var orderReports = await _orderReportRepository.GetAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(orderReports, _httpContextAccessor.HttpContext.Response);

        var orderReportsList = orderReports.ToList();

        var orderReportsResponse = _mapper.Map<List<OrderReportResponse>>(orderReportsList);

        var orderReportsShaped = _dataShapingService.ShapeDataList(orderReportsResponse, request.Parameters.Fields);

        return orderReportsShaped;
    }
}
