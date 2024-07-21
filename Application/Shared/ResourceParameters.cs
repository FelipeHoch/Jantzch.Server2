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

    private string _orderBy = "name";

    public string OrderBy {
        get => _orderBy;
        set {
            if (value != null && value.Contains("-"))
            {
                OrderByDesc = true;
                _orderBy = value.Replace("-", "");
            }
            else
            {
                OrderByDesc = false;
                _orderBy = value;                           
            }
        }
    }

    public bool? OrderByDesc { get; set; }

    public string? Fields { get; set; }
}
