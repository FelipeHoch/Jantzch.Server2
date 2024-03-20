using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Orders;

namespace Jantzch.Server2;

public interface IPotentialOrderRepository
{
    Task<PagedList<PotentialOrder>> GetAsync(PotentialOrderResourceParameters parameters, CancellationToken cancellationToken);

    Task<PotentialOrder?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task AddAsync(PotentialOrder order, CancellationToken cancellationToken);

    Task UpdateAsync(PotentialOrder order, CancellationToken cancellationToken);
}
