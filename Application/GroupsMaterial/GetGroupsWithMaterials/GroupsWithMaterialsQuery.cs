using MediatR;

namespace Jantzch.Server2.Application.GroupsMaterial.GetGroupsWithMaterials;

public record GroupsWithMaterialsQuery() : IRequest<IEnumerable<GroupMaterialResponse>>;
