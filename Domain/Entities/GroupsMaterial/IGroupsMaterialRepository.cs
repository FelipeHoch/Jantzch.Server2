using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Shared;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.GroupsMaterial;

public interface IGroupsMaterialRepository
{
    Task<PagedList<GroupMaterial>> GetGroupsAsync(ResourceParameters parameters, CancellationToken cancellationToken);

    Task<GroupMaterial?> GetGroupByIdAsync(ObjectId id, CancellationToken cancellationToken);

    Task AddGroupAsync(GroupMaterial group, CancellationToken cancellationToken);

    Task UpdateGroupAsync(GroupMaterial group);

    Task DeleteGroupAsync(GroupMaterial group);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
