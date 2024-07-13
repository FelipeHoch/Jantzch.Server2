﻿using Jantzch.Server2.Application.Helpers;

namespace Jantzch.Server2.Domain.Entities.Clients.Deals;

public interface IDealRepository
{
    Task<PagedList<Deal>> GetAsync(DealsResourceParamenters parameters, CancellationToken cancellationToken);

    Task<Deal?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<Deal?> GetByIntegrationIdAsync(string integrationId, CancellationToken cancellationToken);

    Task<List<Deal>> GetByIntegrationIdsAsync(List<string> integrationIds, CancellationToken cancellationToken);

    Task AddAsync(Deal deal, CancellationToken cancellationToken);

    Task AddAsync(IEnumerable<Deal> deals, CancellationToken cancellationToken);

    Task UpdateAsync(Deal deal, CancellationToken cancellationToken);

    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
