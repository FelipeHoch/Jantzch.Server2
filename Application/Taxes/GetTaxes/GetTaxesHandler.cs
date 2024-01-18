using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Taxes;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Taxes.GetTaxes;

public class GetTaxesHandler : IRequestHandler<TaxesQuery, IEnumerable<ExpandoObject>>
{
    private readonly ITaxesRepository _taxesRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetTaxesHandler(
        ITaxesRepository taxesRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _taxesRepository = taxesRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(TaxesQuery request, CancellationToken cancellationToken)
    {
        var taxes = await _taxesRepository.GetTaxesAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(taxes, _httpContextAccessor.HttpContext.Response);

        var taxesList = taxes.ToList();

        var taxesResponse = _mapper.Map<List<TaxResponse>>(taxesList);

        var taxesShaped = _dataShapingService.ShapeDataList(taxesResponse, request.Parameters.Fields);

        return taxesShaped;
    }
}
