using Jantzch.Server2.Application.Abstractions.Google;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infrastructure.Google;
using Jantzch.Server2.Infrastructure.Google.Models;
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
        List<Task<Localization>> localizationTasks = [];

        foreach (var localization in request.Localizations)
        {
            localizationTasks.Add(Task.Run(async () =>
            {
                var geoCode = await _googleMapsService.GetGeoCode(localization.Address);

                Location? location = null;

                if (geoCode is not null)
                {
                    location = new Location
                    {
                        Latitude = geoCode.Results.First().Geometry.Location.Lat,
                        Longitude = geoCode.Results.First().Geometry.Location.Lng
                    };
                }

                Distance? distance = null;

                if (location is not null)
                {
                    distance = await _googleMapsService.GetDistance(location);
                }

                Route? route = null;

                if (distance is not null)
                {
                    route = new Route
                    {
                        Distance = distance.Rows.First().Elements.First().Distance,
                        Duration = distance.Rows.First().Elements.First().Duration
                    };
                }

                return new Localization
                {
                    Address = localization.Address,
                    Location = location,
                    Route = route,
                    IsPrimary = localization.IsPrimary,
                    Description = localization.Description
                };
            }));
        }

        List<Localization> localizations = (await Task.WhenAll(localizationTasks)).ToList();

        var client = new Client
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Cnpj = request.Cnpj,
            Cpf = request.Cpf,
            Types = request.Types,
            Localizations = localizations
        };

        await _clientsRepository.AddAsync(client, cancellationToken);

        return client;
    }
}
