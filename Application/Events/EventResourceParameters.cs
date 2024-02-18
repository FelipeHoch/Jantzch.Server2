using Jantzch.Server2.Application.Shared;

namespace Jantzch.Server2.Application.Events;

public class EventResourceParameters : ResourceParameters
{
    public string? UserIds { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? EventType { get; set; }
}
