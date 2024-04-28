using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Google;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;
using Route = Jantzch.Server2.Domain.Entities.Clients.Route;

namespace Jantzch.Server2.Application.Clients.EditAddress;

public class EditAddressCommandHandler : IRequestHandler<EditAddressCommand.Command, Client>
{
    private readonly IClientsRepository _clientsRepository;

    private readonly IGoogleMapsService _googleMapsService;

    private readonly IMapper _mapper;

    public EditAddressCommandHandler(
        IClientsRepository clientsRepository, 
        IGoogleMapsService googleMapsService,
        IMapper mapper)
    {
        _clientsRepository = clientsRepository;

        _googleMapsService = googleMapsService;

        _mapper = mapper;
    }

    public async Task<Client> Handle(EditAddressCommand.Command request, CancellationToken cancellationToken)
    {
        var client = await _clientsRepository.GetByIdAsync(new ObjectId(request.Id), cancellationToken);

        if (client is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = ClientErrorMessages.NOT_FOUND });
        }

        var address = _mapper.Map<Address>(request.Model);

        var geoCode = await _googleMapsService.GetGeoCode(address);

        if (geoCode is null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new { message = ClientErrorMessages.ADDRESS_NOT_FOUND });
        }

        var location = new Location
        {
            Latitude = geoCode.Results.First().Geometry.Location.Lat,
            Longitude = geoCode.Results.First().Geometry.Location.Lng
        };

        var distance = await _googleMapsService.GetDistance(location);

        if (distance is null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new { message = ClientErrorMessages.INVALID_ADDRESS });
        }

        var route = new Route
        {
            Distance = distance.Rows.First().Elements.First().Distance,
            Duration = distance.Rows.First().Elements.First().Duration
        };

        var localizationToUpdate = client.Localizations.Find(localization => localization.Location.Equals(request.Model.PreviousLocation));

        if (localizationToUpdate is null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new { message = ClientErrorMessages.ADDRESS_NOT_FOUND });
        }

        localizationToUpdate.Address = address;
        localizationToUpdate.Location = location;
        localizationToUpdate.Route = route;

        await _clientsRepository.UpdateAsync(client);

        return client;
    }
}
