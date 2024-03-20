using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;

namespace Jantzch.Server2;

public class CreatePotentialOrderCommand : IRequest<PotentialOrder>
{
    public int EstimatedCompletionTimeInMilliseconds { get; set; }

    public ClientSimple Client { get; set; }

    public string? Observations { get; set; }
}
