namespace Jantzch.Server2.Application.Deals.Analytics;

public class AnalyticsResourceParameters
{
    // start of the year
    public DateTime InitialDate { get; set; } = new DateTime(DateTime.Now.Year, 1, 1);

    public DateTime? FinalDate { get; set; } = DateTime.Now;
}
