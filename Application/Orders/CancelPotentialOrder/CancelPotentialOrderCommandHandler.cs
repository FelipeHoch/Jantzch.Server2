using System.Net;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2;

public class CancelPotentialOrderCommandHandler : IRequestHandler<CancelPotentialOrderCommand>
{
    private readonly IPotentialOrderRepository _potentialOrderRepository;

    public CancelPotentialOrderCommandHandler(IPotentialOrderRepository potentialOrderRepository)
    {
        _potentialOrderRepository = potentialOrderRepository;
    }

    public async Task Handle(CancelPotentialOrderCommand request, CancellationToken cancellationToken)
    {
        var potentialOrder = await _potentialOrderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (potentialOrder is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = PotentialOrderErrorMessages.NOT_FOUND });
        }

        potentialOrder.Cancel();

        await _potentialOrderRepository.UpdateAsync(potentialOrder, cancellationToken);
    }
}
