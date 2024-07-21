using Jantzch.Server2.Application.Deals.Analytics.DTOs;

namespace Jantzch.Server2.Application.Abstractions.Repositories;

public interface IAnalyticsReadRepository
{
    Task<List<DealAnalyticByInstallationType>> GetDealAnalyticByInstallationTypeAsync(DateTime initial, CancellationToken cancellationToken);

    Task<List<DealAnalyticByCity>> GetDealAnalyticByCityAsync(DateTime initial, CancellationToken cancellationToken);

    Task<List<DealAnalyticByMonth>> GetDealAnalyticByMonthAsync(DateTime initial, CancellationToken cancellationToken);
}
