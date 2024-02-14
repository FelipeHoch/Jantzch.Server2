namespace Jantzch.Server2.Application.Shared;

public class ResourceParameters
{
    const int maxPageSize = 1000;
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 1000;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
    public string OrderBy { get; set; } = "name";
    public string? Fields { get; set; }
}
