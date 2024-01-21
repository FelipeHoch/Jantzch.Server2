using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Dynamic;
using System.Net;

namespace Jantzch.Server2.Application.ReportConfigurations.GetReportConfiguration;

public class GetConfigurationHandler : IRequestHandler<ConfigurationQuery, ExpandoObject>
{
    private readonly IReportConfigurationRepository _reportConfigurationRepository;

    private readonly IMapper _mapper;

    private readonly IDataShapingService _dataShapingService;

    public GetConfigurationHandler(
        IReportConfigurationRepository reportConfigurationRepository,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _reportConfigurationRepository = reportConfigurationRepository;
        _mapper = mapper;
        _dataShapingService = dataShapingService;
    }

    public async Task<ExpandoObject> Handle(ConfigurationQuery request, CancellationToken cancellationToken)
    {
        var configuration = await _reportConfigurationRepository.GetByKeyAsync(request.Key, cancellationToken);

        if (configuration is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { ReportConfiguration = Constants.NOT_FOUND });
        }

        var configurationResponse = _mapper.Map<ReportConfigurationResponse>(configuration);

        var shapedConfiguration = _dataShapingService.ShapeData(configurationResponse, request.Fields);

        return shapedConfiguration;
    }
}
