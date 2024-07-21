namespace Jantzch.Server2.Application.Deals.Analytics.DTOs;

public class MonthlySummary
{
    public int TotalDeals { get; set; }
    public double TotalValue { get; set; }
    public double AverageValue { get; set; }
    public string MostCommonInstallationType { get; set; }
}

public class ComparativeIndicators
{
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
    public MonthlySummary CurrentMonthSummary { get; set; }
    public MonthlySummary PreviousMonthSummary { get; set; }
    public PercentageDifference PercentageDifference { get; set; }
}

public class PercentageDifference
{
    public double TotalDeals { get; set; }
    public double TotalValue { get; set; }
    public double AverageValue { get; set; }
}
