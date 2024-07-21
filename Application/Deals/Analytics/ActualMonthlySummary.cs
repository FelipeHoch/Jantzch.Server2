using Jantzch.Server2.Application.Abstractions.Repositories;
using Jantzch.Server2.Application.Abstractions.Services;
using Jantzch.Server2.Application.Deals.Analytics.DTOs;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using MediatR;

namespace Jantzch.Server2.Application.Deals.Analytics;

public class ActualMonthlySummary
{
    public record Query : IRequest<ComparativeIndicators>;

    public class Handler(
        IAnalyticsReadRepository analyticsReadRepository,
        IDealRepository dealRepository,
        IAnalyticService analyticService
    ) : IRequestHandler<Query, ComparativeIndicators>
    {
        public async Task<ComparativeIndicators> Handle(Query request, CancellationToken cancellationToken)
        {
            var initialOfPreviousMonth = DateTime.Now.AddMonths(-1);

            initialOfPreviousMonth = new DateTime(initialOfPreviousMonth.Year, initialOfPreviousMonth.Month, 1);

            var parameters = new AnalyticsResourceParameters
            {
                // init of previous month
                InitialDate = initialOfPreviousMonth,
                FinalDate = DateTime.Now,
            };

            var deals = await dealRepository.GetAsync(parameters, cancellationToken);

            return analyticService.CalculateCompariativeIndicators(deals);
        }
    }
}
