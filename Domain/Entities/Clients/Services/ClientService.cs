using Jantzch.Server2.Application.Abstractions.Google;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infrastructure.Google;
using System.Net;

namespace Jantzch.Server2.Domain.Entities.Clients.Services;

public class ClientService(
    IGoogleMapsService googleMapsService
) : IClientService
{
    public async Task<Localization?> GetLocalization(Address address, bool isPrimary, string description)
    {
        var geoCode = await googleMapsService.GetGeoCode(address);

        if (geoCode is null)
        {
            return null;
        }

        var location = new Location
        {
            Latitude = geoCode.Results.First().Geometry.Location.Lat,
            Longitude = geoCode.Results.First().Geometry.Location.Lng
        };

        var distance = await googleMapsService.GetDistance(location);

        if (distance is null)
        {
            return null;
        }

        var route = new Route
        {
            Distance = distance.Rows.First().Elements.First().Distance,
            Duration = distance.Rows.First().Elements.First().Duration
        };

        return new Localization
        {
            Location = location,
            Address = address,
            Route = route,
            Description = description,
            IsPrimary = isPrimary
        };
    }
}
