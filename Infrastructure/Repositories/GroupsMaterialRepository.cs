using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class GroupsMaterialRepository : IGroupsMaterialRepository
{
    private readonly JantzchContext _context;

    public GroupsMaterialRepository(JantzchContext context)
    {
        _context = context;
    }

    public async Task<GroupMaterial?> GetGroupById(ObjectId id)
    {
        return await _context.GroupMaterials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddGroup(GroupMaterial group)
    {
        await _context.GroupMaterials.AddAsync(group);
    }

    public async Task UpdateGroup(GroupMaterial group)
    {
        _context.GroupMaterials.Update(group);

        await Task.FromResult(Unit.Value);
    }

    public async Task DeleteGroup(GroupMaterial group)
    {
        _context.GroupMaterials.Remove(group);

        await Task.FromResult(Unit.Value);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
