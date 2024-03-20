using Jantzch.Server2.Application.Shared;

namespace Jantzch.Server2.Application.Orders;

public class PotentialOrderResourceParameters : ResourceParameters
{
    public string? CreatedBy { get; set; }
    public string? Status { get; set; }
    public string? Client { get; set; }
    public new string OrderBy { get; set; } = "createdAt";
}
