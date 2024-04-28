using Jantzch.Server2.Application.Clients;
using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Enums;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Dynamic.Core;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly IMongoDatabase _database;

    private readonly IMongoCollection<Client> _clients;

    public ClientsRepository(IMongoDatabase database)
    {
        _database = database;

        _clients = _database.GetCollection<Client>("clients");
    }

    public async Task<PagedList<Client>> GetAsync(ClientsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var filter = BuildClientFilters(parameters);
        var sort = BuildSortDefinition(parameters); 

        var clients = _clients.Aggregate()
            .Match(filter)
            .Sort(sort);

        var count = (int)await _clients.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<Client>.CreateAsync(clients, parameters?.PageNumber ?? 1, parameters?.PageSize ?? 10, count, cancellationToken);
    }

    public async Task<Client?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await _clients.Find(client => client.Id == id.ToString()).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Client client, CancellationToken cancellationToken)
    {
        await _clients.InsertOneAsync(client, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Client client)
    {
        await _clients.ReplaceOneAsync(x => x.Id == client.Id, client);
    }

    public async Task DeleteAsync(Client client)
    {
        await _clients.DeleteOneAsync(x => x.Id == client.Id);
    }

    public async Task<bool> ClientExists(ObjectId id, CancellationToken cancellationToken)
    {
        return await _clients.Find(client => client.Id == id.ToString()).AnyAsync(cancellationToken);
    }

    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private static FilterDefinition<Client> BuildClientFilters(ClientsResourceParameters clientsResourceParameters)
    {
        var builder = Builders<Client>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(clientsResourceParameters.City))
        {
            var cityFilter = builder.Exists(client => client.Localizations.Any(localization => localization.Address.City.ToLower() == clientsResourceParameters.City.ToLower()));
            filter = cityFilter;
        }

        if (!string.IsNullOrWhiteSpace(clientsResourceParameters.State))
        {
            var stateFilter = builder.Exists(client => client.Localizations.Any(localization => localization.Address.State.ToLower() == clientsResourceParameters.State.ToLower()));
            filter = stateFilter;
        }

        if (!string.IsNullOrWhiteSpace(clientsResourceParameters.Type))
        {
            var types = clientsResourceParameters.Type.Split(',')
            .Select(type => (ClientType)Enum.Parse(typeof(ClientType), type))
            .ToArray();

            var typeFilter = builder.AnyIn(client => client.Types, types);
        }

        if (!string.IsNullOrWhiteSpace(clientsResourceParameters.SearchQuery))
        {
            var searchQuery = clientsResourceParameters.SearchQuery.Trim().ToLower();

            var where = builder.Or(
                 builder.Regex(client => client.Name, "/^" + searchQuery + "/i"),
                 builder.Regex(client => client.Email, "/^" + searchQuery + "/i")
                );

            filter &= where;
        }

        return filter;
    }

    private static SortDefinition<Client> BuildSortDefinition(ClientsResourceParameters clientsResourceParameters)
    {
        var sort = Builders<Client>.Sort.Descending(client => client.Name);

        if (!string.IsNullOrWhiteSpace(clientsResourceParameters.OrderBy))
        {           
            sort = Builders<Client>.Sort.Ascending(clientsResourceParameters.OrderBy);
        }

        return sort;
    }
}
