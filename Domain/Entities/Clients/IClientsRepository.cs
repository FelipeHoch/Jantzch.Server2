using Jantzch.Server2.Application.Clients;
using Jantzch.Server2.Application.Helpers;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.Clients;

public interface IClientsRepository
{
    Task<PagedList<Client>> GetAsync(ClientsResourceParameters parameters, CancellationToken cancellationToken);

    Task<Client?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken);

    Task AddAsync(Client client, CancellationToken cancellationToken);

    Task UpdateAsync(Client client);

    Task DeleteAsync(Client client);

    Task<bool> ClientExists(ObjectId id, CancellationToken cancellationToken);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
