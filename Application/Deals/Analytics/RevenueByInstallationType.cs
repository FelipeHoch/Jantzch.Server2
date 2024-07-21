using Jantzch.Server2.Application.Abstractions.Repositories;
using Jantzch.Server2.Application.Deals.Analytics.DTOs;
using MediatR;

namespace Jantzch.Server2.Application.Deals.Analytics;

public class RevenueByInstallationType
{
    public record Query(AnalyticsResourceParameters Parameters) : IRequest<IEnumerable<DealAnalyticByInstallationType>>;

    public class Handler(
        IAnalyticsReadRepository analyticsReadRepository
    ) : IRequestHandler<Query, IEnumerable<DealAnalyticByInstallationType>>
    {
        public async Task<IEnumerable<DealAnalyticByInstallationType>> Handle(Query request, CancellationToken cancellationToken)
        {
            var initial = request.Parameters.InitialDate;

            return await analyticsReadRepository.GetDealAnalyticByInstallationTypeAsync(initial, cancellationToken);
        }
    }
}
