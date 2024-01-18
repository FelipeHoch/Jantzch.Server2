using Jantzch.Server2.Application.Shared;

namespace Jantzch.Server2.Application.Taxes;

public class TaxesResourceParameters : ResourceParameters
{
    public string? Type { get; set; }
}
