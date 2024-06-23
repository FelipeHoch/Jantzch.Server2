using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.Clients.DeleteAddress;

public class DeleteAddress
{
    public record Command(string ClientId, string Id) : IRequest<Client>;

    public class Handler(IClientsRepository clientsRepository) : IRequestHandler<Command, Client>
    {
        public async Task<Client> Handle(Command request, CancellationToken cancellationToken)
        {
            var client = await clientsRepository.GetByIdAsync(new ObjectId(request.ClientId), cancellationToken);

            if (client is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = ClientErrorMessages.NOT_FOUND });
            }

            client.RemoveLocalization(request.Id);

            await clientsRepository.UpdateAsync(client);

            return client;
        }
    }

}
