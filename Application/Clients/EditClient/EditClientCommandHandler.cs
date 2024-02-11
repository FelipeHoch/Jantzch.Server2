using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Clients.EditClient;

public class EditClientCommandHandler : IRequestHandler<EditClientCommand.Command, Client>
{
    private readonly IClientsRepository _clientsRepository;

    public EditClientCommandHandler(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task<Client> Handle(EditClientCommand.Command request, CancellationToken cancellationToken)
    {
        var client = await _clientsRepository.GetByIdAsync(new ObjectId(request.Id), cancellationToken);

        if (client is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = ClientErrorMessages.NOT_FOUND });
        }

        client.Name = request.Model.Name;
        client.Email = request.Model.Email;
        client.Cnpj = request.Model.Cnpj;
        client.Cpf = request.Model.Cpf;
        client.PhoneNumber = request.Model.PhoneNumber;

        await _clientsRepository.UpdateAsync(client);

        return client;
    }
}
