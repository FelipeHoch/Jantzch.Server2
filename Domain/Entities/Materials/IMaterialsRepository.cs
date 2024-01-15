using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Features.Materials;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.Materials;

public interface IMaterialsRepository
{
    Task<PagedList<MaterialDTO>> GetMaterials(MaterialsResourceParameters parameters);

    Task<Material?> GetMaterialById(ObjectId id);

    Task AddMaterial(Material material);

    Task UpdateMaterial(Material material);

    Task DeleteMaterial(Material material);

    Task<bool> SaveChangesAsync();
}
