using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Events;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Events.GetEvents;

public class GetEventsHandler : IRequestHandler<EventsQuery, IEnumerable<ExpandoObject>>
{
    private readonly IEventRepository _eventRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetEventsHandler(
        IEventRepository eventRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(EventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(events, _httpContextAccessor.HttpContext.Response);

        var eventsList = events.ToList();

        var eventsResponse = _mapper.Map<List<EventResponse>>(eventsList);

        var eventsShaped = _dataShapingService.ShapeDataList(eventsResponse, request.Parameters.Fields);

        return eventsShaped;
    }
}
