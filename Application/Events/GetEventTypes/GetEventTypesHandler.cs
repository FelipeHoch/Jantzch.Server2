using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Events;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Events.GetEventTypes;

public class GetEventTypesHandler : IRequestHandler<EventTypesQuery, IEnumerable<ExpandoObject>>
{
    private readonly IEventTypeRepository _eventTypeRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetEventTypesHandler(
        IEventTypeRepository eventTypeRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _eventTypeRepository = eventTypeRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(EventTypesQuery request, CancellationToken cancellationToken)
    {
        var eventTypes = await _eventTypeRepository.GetAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(eventTypes, _httpContextAccessor.HttpContext.Response);

        var eventTypesList = eventTypes.ToList();

        var eventTypesResponse = _mapper.Map<List<EventTypeResponse>>(eventTypesList);

        var eventTypesShaped = _dataShapingService.ShapeDataList(eventTypesResponse, request.Parameters.Fields);

        return eventTypesShaped;
    }
}
