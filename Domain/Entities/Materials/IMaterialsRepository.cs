using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Materials;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.Materials;

public interface IMaterialsRepository
{
    Task<PagedList<Material>> GetMaterialsAsync(MaterialsResourceParameters parameters, CancellationToken cancellationToken);

    Task<Material?> GetMaterialByIdAsync(ObjectId id);

    Task AddMaterialAsync(Material material);

    Task UpdateMaterialAsync(Material material);

    Task DeleteMaterialAsync(Material material);

    Task<bool> SaveChangesAsync();
}
