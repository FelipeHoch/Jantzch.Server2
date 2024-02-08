using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Taxes;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.Taxes;

public interface ITaxesRepository
{
    Task<PagedList<Tax>> GetAsync(TaxesResourceParameters parameters, CancellationToken cancellationToken);

    Task<Tax?> LastTaxInsertedAsync(CancellationToken cancellationToken);

    Task UpdateAsync(Tax tax);

    Task AddAsync(Tax tax, CancellationToken cancellationToken);

    Task DeleteAsync(Tax tax);

    Task<Tax?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken);

    Task<List<Tax>> GetByIds(List<ObjectId> ids, CancellationToken cancellationToken);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
