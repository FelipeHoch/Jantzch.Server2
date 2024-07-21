using Jantzch.Server2.Application.Abstractions.Repositories;
using Jantzch.Server2.Application.Deals.Analytics.DTOs;
using MediatR;

namespace Jantzch.Server2.Application.Deals.Analytics;

public class RevenueByCity
{
    public record Query(AnalyticsResourceParameters Parameters) : IRequest<IEnumerable<DealAnalyticByCity>>;

    public class Handler(
        IAnalyticsReadRepository analyticsReadRepository
    ) : IRequestHandler<Query, IEnumerable<DealAnalyticByCity>>
    {
        public async Task<IEnumerable<DealAnalyticByCity>> Handle(Query request, CancellationToken cancellationToken)
        {
            var initial = request.Parameters.InitialDate;

            return await analyticsReadRepository.GetDealAnalyticByCityAsync(initial, cancellationToken);
        }
    }
}
