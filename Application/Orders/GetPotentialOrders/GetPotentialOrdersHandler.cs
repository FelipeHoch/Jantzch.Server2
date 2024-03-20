using System.Dynamic;
using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using MediatR;

namespace Jantzch.Server2;

public class GetPotentialOrdersHandler : IRequestHandler<PotentialOrdersQuery, IEnumerable<ExpandoObject>>
{
    private readonly IPotentialOrderRepository _potentialOrderRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetPotentialOrdersHandler(
        IPotentialOrderRepository potentialOrderRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
        )
    {
        _potentialOrderRepository = potentialOrderRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(PotentialOrdersQuery request, CancellationToken cancellationToken)
    {
        var potentialOrders = await _potentialOrderRepository.GetAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(potentialOrders, _httpContextAccessor.HttpContext.Response);

        var potentialOrdersList = potentialOrders.ToList();

        var potentialOrdersResponse = _mapper.Map<List<PotentialOrderResponse>>(potentialOrdersList);

        var potentialOrdersShaped = _dataShapingService.ShapeDataList(potentialOrdersResponse, request.Parameters.Fields);

        return potentialOrdersShaped;
    }
}
