using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Clients.DeleteClient;

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand>
{
    private readonly IClientsRepository _clientsRepository;

    public DeleteClientCommandHandler(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientsRepository.GetByIdAsync(new ObjectId(request.Id), cancellationToken);

        if (client is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { Client = Constants.NOT_FOUND });
        }

        await _clientsRepository.DeleteAsync(client);

        await Task.FromResult(Unit.Value);
    }
}
