using Jantzch.Server2.Application.Abstractions.Services;
using Jantzch.Server2.Application.Deals.Analytics.DTOs;
using Jantzch.Server2.Domain.Entities.Clients.Deals;

namespace Jantzch.Server2.Infrastructure.Services;

public class AnalyticService : IAnalyticService
{
    public MonthlySummary CalculateMonthlySummary(List<Deal> deals)
    {
        var totalDeals = deals.Count;

        var totalValue = deals.Sum(deal => deal.Value);

        var avgValue = totalDeals > 0 ? totalValue / totalDeals : 0;

        var installationType = deals
            .GroupBy(d => d.InstalationType)
            .ToDictionary(type => type.Key, type => type.Count());

        var mostInstalationType = installationType.Any() ? installationType.Aggregate((l, r) => l.Value > r.Value ? l : r).Key : "N/A";

        return new MonthlySummary
        {
            TotalDeals = totalDeals,
            TotalValue = totalValue,
            AverageValue = avgValue,
            MostCommonInstallationType = mostInstalationType
        };
    }

    public ComparativeIndicators CalculateCompariativeIndicators(List<Deal> deals)
    {
        var initialActualMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        var endActualMonth = initialActualMonth.AddMonths(1).AddSeconds(-1);

        var initialPreviousMonth = initialActualMonth.AddMonths(-1);

        var endPreviousMonth = initialActualMonth.AddSeconds(-1);

        var actualDeals = deals.Where(d => d.DealConfirmedAt >= initialActualMonth && d.DealConfirmedAt <= endActualMonth).ToList();

        var previousDeals = deals.Where(d => d.DealConfirmedAt >= initialPreviousMonth && d.DealConfirmedAt <= endPreviousMonth).ToList();

        var actualSummary = CalculateMonthlySummary(actualDeals);

        var previousSummary = CalculateMonthlySummary(previousDeals);

        var totalDealsDifference = CalculatePercentualDifference(actualSummary.TotalDeals, previousSummary.TotalDeals);
        
        var totalValueDifference = CalculatePercentualDifference(actualSummary.TotalValue, previousSummary.TotalValue);

        var avgValueDifference = CalculatePercentualDifference(actualSummary.AverageValue, previousSummary.AverageValue);

        return new ComparativeIndicators
        {
            InitialDate = initialActualMonth,
            FinalDate = endActualMonth,
            CurrentMonthSummary = actualSummary,
            PreviousMonthSummary = previousSummary,
            PercentageDifference = new PercentageDifference
            {
                TotalDeals = totalDealsDifference,
                TotalValue = totalValueDifference,
                AverageValue = avgValueDifference
            }
        };
    }

    public double CalculatePercentualDifference(double actualValue, double previousValue)
    {
        if (previousValue == 0)
        {
            return actualValue > 0 ? double.PositiveInfinity : 0;
        }

        return (actualValue - previousValue) / previousValue * 100;
    }
}
