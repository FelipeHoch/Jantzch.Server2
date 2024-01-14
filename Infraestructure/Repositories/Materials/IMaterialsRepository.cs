using Jantzch.Server2.Domain;
using MongoDB.Bson;

namespace Jantzch.Server2.Infraestructure.Repositories.Materials;

public interface IMaterialsRepository
{
    Task<Material?> GetMaterialById(ObjectId id);

    Task AddMaterial(Material material);

    Task UpdateMaterial(Material material);

    Task DeleteMaterial(Material material);

    Task<bool> SaveChangesAsync();
}
