namespace Jantzch.Server2.Application.Deals.Analytics.DTOs;

public class DealAnalyticByMonth
{    
    public double TotalRevenue { get; set; }
    public string Month { get; set; }
    public DateTime Date { get; set; }
    public int Count { get; set; }
}
