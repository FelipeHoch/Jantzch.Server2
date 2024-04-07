using Jantzch.Server2.Application.Shared;

namespace Jantzch.Server2.Application.Clients;

public class ClientsResourceParameters : ResourceParameters
{
    public string? Type { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
}
