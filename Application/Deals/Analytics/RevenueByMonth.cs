using Jantzch.Server2.Application.Abstractions.Repositories;
using Jantzch.Server2.Application.Deals.Analytics.DTOs;
using MediatR;

namespace Jantzch.Server2.Application.Deals.Analytics;

public class RevenueByMonth
{
    public record Query(AnalyticsResourceParameters Parameters) : IRequest<IEnumerable<DealAnalyticByMonth>>;

    public class Handler(
        IAnalyticsReadRepository analyticsReadRepository
    ) : IRequestHandler<Query, IEnumerable<DealAnalyticByMonth>>
    {
        public async Task<IEnumerable<DealAnalyticByMonth>> Handle(Query request, CancellationToken cancellationToken)
        {
            var initial = request.Parameters.InitialDate;

            return await analyticsReadRepository.GetDealAnalyticByMonthAsync(initial, cancellationToken);
        }
    }
}
