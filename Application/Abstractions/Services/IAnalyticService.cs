using Jantzch.Server2.Application.Deals.Analytics.DTOs;
using Jantzch.Server2.Domain.Entities.Clients.Deals;

namespace Jantzch.Server2.Application.Abstractions.Services;

public interface IAnalyticService
{
    MonthlySummary CalculateMonthlySummary(List<Deal> deals);

    ComparativeIndicators CalculateCompariativeIndicators(List<Deal> deals);

    double CalculatePercentualDifference(double actualValue, double previousValue);
}
