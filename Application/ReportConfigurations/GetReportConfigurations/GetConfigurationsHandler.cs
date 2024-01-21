using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.ReportConfigurations.GetReportConfigurations;

public class GetConfigurationsHandler : IRequestHandler<ConfigurationsQuery, IEnumerable<ExpandoObject>>
{
    private readonly IReportConfigurationRepository _reportConfigurationRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetConfigurationsHandler(
        IReportConfigurationRepository reportConfigurationRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _reportConfigurationRepository = reportConfigurationRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(ConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var configurations = await _reportConfigurationRepository.GetAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(configurations, _httpContextAccessor.HttpContext.Response);

        var configurationsList = configurations.ToList();

        var configurationsResponse = _mapper.Map<List<ReportConfigurationResponse>>(configurationsList);

        var configurationsShaped = _dataShapingService.ShapeDataList(configurationsResponse, request.Parameters.Fields);

        return configurationsShaped;
    }
}
