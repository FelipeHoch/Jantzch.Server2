using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Infrastructure.Google.Models;

namespace Jantzch.Server2.Application.Abstractions.Google;

public interface IGoogleMapsService
{
    Task<Distance> GetDistance(Location location);
    Task<GeoCode> GetGeoCode(Address address);
}