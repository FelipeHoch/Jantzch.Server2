using System.Net;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;

namespace Jantzch.Server2;

public class EditPotentialOrderCommandHandler : IRequestHandler<EditPotentialOrderCommand.Command, PotentialOrder>
{
    private readonly IPotentialOrderRepository _potentialOrderRepository;

    private readonly IClientsRepository _clientsRepository;

    public EditPotentialOrderCommandHandler(IPotentialOrderRepository potentialOrderRepository, IClientsRepository clientsRepository)
    {
        _potentialOrderRepository = potentialOrderRepository;
        _clientsRepository = clientsRepository;
    }

    public async Task<PotentialOrder> Handle(EditPotentialOrderCommand.Command request, CancellationToken cancellationToken)
    {
        var potentialOrder = await _potentialOrderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (potentialOrder is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = PotentialOrderErrorMessages.NOT_FOUND });
        }

        var client = await _clientsRepository.GetByIdAsync(new ObjectId(request.Model.Client.Id), cancellationToken);

        if (client is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = ClientErrorMessages.NOT_FOUND });
        }

        potentialOrder.Client = request.Model.Client;
        potentialOrder.EstimatedCompletionTimeInMilliseconds = request.Model.EstimatedCompletionTimeInMilliseconds;
        potentialOrder.Observations = request.Model.Observations;

        await _potentialOrderRepository.UpdateAsync(potentialOrder, cancellationToken);

        return potentialOrder;
    }
}
