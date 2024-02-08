using AutoMapper;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using MediatR;

namespace Jantzch.Server2.Application.GroupsMaterial.GetGroupsWithMaterials;

public class GetGroupsWithMaterialsHandler : IRequestHandler<GroupsWithMaterialsQuery, IEnumerable<GroupMaterialResponse>>
{
    private readonly IGroupsMaterialRepository _repository;
    private readonly IMapper _mapper;

    public GetGroupsWithMaterialsHandler(IGroupsMaterialRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupMaterialResponse>> Handle(GroupsWithMaterialsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _repository.GetGroupsWithMaterialsAsync(cancellationToken);

        return _mapper.Map<IEnumerable<GroupMaterialResponse>>(groups);
    }
}
