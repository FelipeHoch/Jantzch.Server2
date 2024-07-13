using Jantzch.Server2.Application.Clients;
using Jantzch.Server2.Application.Helpers;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.Clients;

public interface IClientsRepository
{
    Task<PagedList<Client>> GetAsync(ClientsResourceParameters parameters, CancellationToken cancellationToken);

    Task<Client?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken);

    Task<List<Client>> GetByMultipleIdentifiersAsync(List<string> emails, List<string> phones, List<string> names, CancellationToken cancellation);

    Task AddAsync(Client client, CancellationToken cancellationToken);

    Task AddAsync(IEnumerable<Client> clients, CancellationToken cancellationToken);

    Task UpdateAsync(Client client);

    Task DeleteAsync(Client client);

    Task<bool> ClientExists(ObjectId id, CancellationToken cancellationToken);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);

    Task ScriptToUpdateAddressToLocalization(); 
}
