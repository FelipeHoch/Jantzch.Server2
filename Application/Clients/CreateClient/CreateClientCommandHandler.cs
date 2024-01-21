using Jantzch.Server2.Application.Abstractions.Google;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infrastructure.Google;
using MediatR;
using System.Net;
using Route = Jantzch.Server2.Domain.Entities.Clients.Route;

namespace Jantzch.Server2.Application.Clients.CreateClient;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Client>
{
    private readonly IClientsRepository _clientsRepository;

    private readonly IGoogleMapsService _googleMapsService;

    public CreateClientCommandHandler(IClientsRepository clientsRepository, IGoogleMapsService googleMapsService)
    {
        _clientsRepository = clientsRepository;
        _googleMapsService = googleMapsService;
    }

    public async Task<Client> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var geoCode = await _googleMapsService.GetGeoCode(request.Address);

        if (geoCode is null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new { Client = "Address not found" });
        }

        var location = new Location
        {
            Latitude = geoCode.Results.First().Geometry.Location.Lat,
            Longitude = geoCode.Results.First().Geometry.Location.Lng
        };

        var distance = await _googleMapsService.GetDistance(location);

        if (distance is null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new { Client = "Distance not found" });
        }

        var route = new Route
        {
            Distance = distance.Rows.First().Elements.First().Distance,
            Duration = distance.Rows.First().Elements.First().Duration
        };

        var client = new Client
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Cnpj = request.Cnpj,
            Cpf = request.Cpf,
            Address = request.Address,
            Location = location,
            Route = route
        };

        await _clientsRepository.AddAsync(client, cancellationToken);

        await _clientsRepository.SaveChangesAsync(cancellationToken);

        return client;
    }
}
