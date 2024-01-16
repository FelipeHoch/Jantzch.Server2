using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using MongoDB.Bson;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Application.Materials;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class MaterialsRepository : IMaterialsRepository
{
    private readonly JantzchContext _context;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public MaterialsRepository(JantzchContext context, IPropertyCheckerService propertyCheckerService)
    {
        _context = context;

        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<Material>> GetMaterialsAsync(MaterialsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var query = _context.Materials.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {
            query = query.Where(x => x.Name.Contains(parameters.SearchQuery));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Group))
        {
            query = query.Where(x => x.GroupIdObject.Equals(ObjectId.Parse(parameters.Group)));
        }

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy) && _propertyCheckerService.TypeHasProperties<Material>(parameters.OrderBy))
        {         
            query = query.OrderBy(parameters.OrderBy + " descending");
        }
        
       
        return await PagedList<Material>.CreateAsync(query, parameters.PageNumber, parameters.PageSize, cancellationToken);
    }   

    public async Task<Material?> GetMaterialByIdAsync(ObjectId id)
    {
        return await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => id.Equals(id));
    }

    public async Task AddMaterialAsync(Material material)
    {
        await _context.Materials.AddAsync(material);
    }

    public async Task UpdateMaterialAsync(Material material)
    {
        _context.Materials.Update(material);

        await Task.FromResult(Unit.Value);
    }

    public async Task DeleteMaterialAsync(Material material)
    {
        _context.Materials.Remove(material);

        await Task.FromResult(Unit.Value);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
