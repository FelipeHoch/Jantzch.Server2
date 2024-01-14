using Jantzch.Server2.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Jantzch.Server2.Infraestructure.Repositories.Materials;

public class MaterialsRepository : IMaterialsRepository
{
    private readonly JantzchContext _context;

    public MaterialsRepository(JantzchContext context)
    {
        _context = context;
    }

    public async Task<Material?> GetMaterialById(ObjectId id)
    {
        return await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
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
