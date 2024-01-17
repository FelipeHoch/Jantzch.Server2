using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Application.Shared;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using MongoDB.Bson;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class GroupsMaterialRepository : IGroupsMaterialRepository
{
    private readonly JantzchContext _context;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public GroupsMaterialRepository(
        JantzchContext context,
        IPropertyCheckerService propertyCheckerService)
    {
        _context = context;

        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<GroupMaterial>> GetGroupsAsync(ResourceParameters parameters, CancellationToken cancellationToken)
    {
        var query = _context.GroupMaterials.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {
            query = query.Where(x => x.Name.Contains(parameters.SearchQuery));
        }

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy) && _propertyCheckerService.TypeHasProperties<GroupMaterial>(parameters.OrderBy))
        {
            query = query.OrderBy(parameters.OrderBy + " descending");
        }

        return await PagedList<GroupMaterial>.CreateAsync(query, parameters?.PageNumber ?? 1, parameters?.PageSize ?? 10, cancellationToken);
    }

    public async Task<GroupMaterial?> GetGroupByIdAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await _context.GroupMaterials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddGroupAsync(GroupMaterial group, CancellationToken cancellationToken)
    {
        await _context.GroupMaterials.AddAsync(group, cancellationToken);

        await Task.FromResult(Unit.Value);
    }

    public async Task UpdateGroupAsync(GroupMaterial group)
    {
        _context.GroupMaterials.Update(group);

        await Task.FromResult(Unit.Value);
    }

    public async Task DeleteGroupAsync(GroupMaterial group)
    {
        _context.GroupMaterials.Remove(group);

        await Task.FromResult(Unit.Value);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
