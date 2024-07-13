using Jantzch.Server2.Application.Shared;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;

namespace Jantzch.Server2.Domain.Entities.Clients.Deals;

public class DealsResourceParamenters : ResourceParameters
{
    public string? ClientId { get; set; }

    public StatusEnum? Status { get; set; }
    
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
