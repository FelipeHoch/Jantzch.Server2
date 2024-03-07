using System.Dynamic;
using System.Net;
using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Application.Events;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Users.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;

namespace Jantzch.Server2;

public class GetEventsByUserHandler : IRequestHandler<EventsByUserQuery, IEnumerable<ExpandoObject>>
{
    private readonly IEventRepository _eventRepository;

    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    private readonly IJwtService _jwtService;

    public GetEventsByUserHandler(
        IEventRepository eventRepository,
        IUserRepository userRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService,
        IJwtService jwtService
    )
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
        _jwtService = jwtService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(EventsByUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _jwtService.GetNameIdentifierFromToken();

        var user = await _userRepository.GetByIdpIdAsync(new ObjectId(userId), cancellationToken);

        if (user is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = UserErrorMessages.NOT_FOUND });
        }

        var events = await _eventRepository.GetByUserAsync(user.Id.ToString(), request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(events, _httpContextAccessor.HttpContext.Response);

        var eventsList = events.ToList();

        var eventsResponse = _mapper.Map<List<EventResponse>>(eventsList);

        var eventsShaped = _dataShapingService.ShapeDataList(eventsResponse, request.Parameters.Fields);

        return eventsShaped;
    }
}
