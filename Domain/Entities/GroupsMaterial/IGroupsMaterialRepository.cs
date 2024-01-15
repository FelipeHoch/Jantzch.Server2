using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.GroupsMaterial;

public interface IGroupsMaterialRepository
{
    Task<GroupMaterial?> GetGroupById(ObjectId id);

    Task AddGroup(GroupMaterial group);

    Task UpdateGroup(GroupMaterial group);

    Task DeleteGroup(GroupMaterial group);

    Task<bool> SaveChangesAsync();
}
