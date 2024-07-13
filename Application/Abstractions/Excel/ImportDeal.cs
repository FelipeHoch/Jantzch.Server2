using Jantzch.Server2.Domain.Entities.Clients;

namespace Jantzch.Server2.Application.Abstractions.Excel;

public class ImportDeal
{
    public string Type { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public double Value { get; set; } = 0;

    public string InstalationType { get; set; } = string.Empty;

    public string StructureType { get; set; } = string.Empty;

    public Address? Address { get; set; }

    public string Phase { get; set; } = string.Empty;

    public string IntegrationId { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? CEP { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; } = string.Empty;

    public string ClientName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? DealConfirmedAt { get; set; }
}
