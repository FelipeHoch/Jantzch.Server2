using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Features.Materials;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using MongoDB.Bson;
using Jantzch.Server2.Application.Services.PropertyChecker;

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

    public async Task<PagedList<MaterialDTO>> GetMaterials(MaterialsResourceParameters parameters)
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

        IQueryable<MaterialDTO> queryWithShaping;

        if (!string.IsNullOrWhiteSpace(parameters.Fields) && _propertyCheckerService.TypeHasProperties<Material>(parameters.Fields))
        {
            var selectInString = "new (" + parameters.Fields + ")";

            queryWithShaping = query.Select<MaterialDTO>(selectInString);
        }
        else
        {
            queryWithShaping = query.Select(mbox => new MaterialDTO
            {
                Id = mbox.Id.Value.ToString(),
            });
        }
       
        return await PagedList<MaterialDTO>.CreateAsync(queryWithShaping, parameters.PageNumber, parameters.PageSize);
    }   

    public async Task<Material?> GetMaterialById(ObjectId id)
    {
        return await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Value.Equals(id));
    }

    public async Task AddMaterial(Material material)
    {
        await _context.Materials.AddAsync(material);
    }

    public async Task UpdateMaterial(Material material)
    {
        _context.Materials.Update(material);

        await Task.FromResult(Unit.Value);
    }

    public async Task DeleteMaterial(Material material)
    {
        _context.Materials.Remove(material);

        await Task.FromResult(Unit.Value);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
