using AutoMapper;
using Jantzch.Server2.Application.Services.DataShapingService;
using Jantzch.Server2.Application.Services.Pagination;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.GroupsMaterial.GetGroupsMaterial;

public class GetGroupsMaterialHandler : IRequestHandler<GroupsMaterialQuery, IEnumerable<ExpandoObject>>
{
    private readonly IGroupsMaterialRepository _groupsMaterialRepository;

    private readonly IMapper _mapper;

    private readonly IPaginationService _paginationService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IDataShapingService _dataShapingService;

    public GetGroupsMaterialHandler(
        IGroupsMaterialRepository groupsMaterialRepository,
        IPaginationService paginationService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IDataShapingService dataShapingService
    )
    {
        _groupsMaterialRepository = groupsMaterialRepository;
        _mapper = mapper;
        _paginationService = paginationService;
        _httpContextAccessor = httpContextAccessor;
        _dataShapingService = dataShapingService;
    }

    public async Task<IEnumerable<ExpandoObject>> Handle(GroupsMaterialQuery request, CancellationToken cancellationToken)
    {
        var groupsMaterial = await _groupsMaterialRepository.GetGroupsAsync(request.Parameters, cancellationToken);

        _paginationService.AddPaginationMetadataInResponseHeader(groupsMaterial, _httpContextAccessor.HttpContext.Response);

        var groupsMaterialList = groupsMaterial.ToList();

        var groupsMaterialDto = _mapper.Map<List<GroupMaterialResponse>>(groupsMaterialList);

        var groupsMaterialShaped = _dataShapingService.ShapeDataList(groupsMaterialDto, request.Parameters.Fields);

        return groupsMaterialShaped;
    }
}