using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<UsersQuery, IEnumerable<ExpandoObject>>
{
    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetUsersHandler(
        IUserRepository userRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(UsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(users, _httpContextAccessor.HttpContext.Response);

        var usersList = users.ToList();

        var usersResponse = _mapper.Map<List<UserResponse>>(usersList);

        var usersShaped = _dataShapingService.ShapeDataList(usersResponse, request.Parameters.Fields);

        return usersShaped;
    }
}
