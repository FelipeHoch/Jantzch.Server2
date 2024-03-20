using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;

namespace Jantzch.Server2;

public class EditPotentialOrderCommand
{
    public ClientSimple Client { get; set; }

    public int EstimatedCompletionTimeInMilliseconds { get; set; }

    public string? Observations { get; set; }

    public record Command(string Id, EditPotentialOrderCommand Model) : IRequest<PotentialOrder>;
}
