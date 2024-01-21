using Jantzch.Server2.Application.Clients;
using Jantzch.Server2.Application.Clients.GetClientsInformation;
using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Dynamic.Core;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly JantzchContext _context;

    private readonly IMongoDatabase _database;

    private readonly IMongoCollection<Client> _clients;

    public ClientsRepository(JantzchContext context, IMongoDatabase database)
    {
        _context = context;

        _database = database;

        _clients = _database.GetCollection<Client>("clients");
    }

    public async Task<PagedList<Client>> GetAsync(ClientsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var builder = Builders<Client>.Filter;
        var filter = BuildClientFilters(parameters);
        var sort = BuildSortDefinition(parameters); 

        var clients = _clients.Aggregate()
            .Match(filter)
            .Sort(sort);

        var count = (int)await _clients.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<Client>.CreateAsync(clients, parameters?.PageNumber ?? 1, parameters?.PageSize ?? 10, count, cancellationToken);
    }

    public async Task<PagedList<ClientInformationResponse>> GetInformationsAsync(ClientsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var builder = Builders<Client>.Filter;
        var filter = BuildClientFilters(parameters);
        var sort = BuildSortDefinition(parameters);
        
        var query = _clients.Aggregate()
            .Match(filter)
            .Sort(sort)
            .Project(client => new ClientInformationResponse
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Address = client.Address,
                Location = client.Location
            });

        var count = (int)await _clients.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<ClientInformationResponse>.CreateAsync(query, parameters?.PageNumber ?? 1, parameters?.PageSize ?? 10, count, cancellationToken);
    }

    public async Task<Client?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.ToString(), cancellationToken);
    }

    public async Task AddAsync(Client client, CancellationToken cancellationToken)
    {
        await _clients.InsertOneAsync(client);
    }

    public async Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);

        await Task.FromResult(Unit.Value);
    }

    public async Task DeleteAsync(Client client)
    {
        _context.Clients.Remove(client);

        await Task.FromResult(Unit.Value);
    }

    public async Task<bool> ClientExists(ObjectId id, CancellationToken cancellationToken)
    {
        return await _context.Clients.AnyAsync(x => x.Id == id.ToString(), cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    private FilterDefinition<Client> BuildClientFilters(ClientsResourceParameters clientsResourceParameters)
    {
        var builder = Builders<Client>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(clientsResourceParameters.City))
        {
            var cityFilter = builder.Eq(client => client.Address.City.ToLower(), clientsResourceParameters.City.ToLower());
            filter = cityFilter;
        }

        if (!string.IsNullOrWhiteSpace(clientsResourceParameters.State))
        {
            var stateFilter = builder.Eq(client => client.Address.State.ToLower(), clientsResourceParameters.State.ToLower());
            filter = stateFilter;
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

    private SortDefinition<Client> BuildSortDefinition(ClientsResourceParameters clientsResourceParameters)
    {
        var sort = Builders<Client>.Sort.Descending(client => client.Name);

        if (!string.IsNullOrWhiteSpace(clientsResourceParameters.OrderBy))
        {           
            sort = Builders<Client>.Sort.Ascending(clientsResourceParameters.OrderBy);
        }

        return sort;
    }
}
