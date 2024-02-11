using Jantzch.Server2.Application.Abstractions.Google;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Infraestructure.Errors;
using Jantzch.Server2.Infrastructure.Google.Constants;
using Jantzch.Server2.Infrastructure.Google.Models;
using System.Globalization;
using System.Text.Json;

namespace Jantzch.Server2.Infrastructure.Google;

public sealed class GoogleMapsService : IGoogleMapsService
{
    private readonly HttpClient _client;

    public GoogleMapsService(HttpClient client)
    {
        _client = client;
    }

    public async Task<GeoCode> GetGeoCode(Address address)
    {
        var queryParams = BuildQueryParams(address);

        var response = await _client.GetFromJsonAsync<GeoCode>($"geocode/json?{queryParams}");

        if (response is null)
        {
            throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = GoogleErrorMessages.GEO_CODE });
        }

        return response;
    }

    public async Task<Distance> GetDistance(Location location)
    {
        var queryParams = BuildQueryParams(location);

        var response = await _client.GetFromJsonAsync<Distance>($"distancematrix/json?{queryParams}");

        if (response is null)
        {
            throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = GoogleErrorMessages.GEO_CODE });
        }

        return response;
    }

    private static string BuildQueryParams(Location location)
    {
        var googleMapsToken = Environment.GetEnvironmentVariable("GOOGLE_MAPS_TOKEN");

        if (string.IsNullOrEmpty(googleMapsToken))
        {
            throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = GoogleErrorMessages.GEO_CODE });
        }

        var destinations = $"{location.Latitude.ToString(CultureInfo.InvariantCulture)},{location.Longitude.ToString(CultureInfo.InvariantCulture)}";
        var origins = "-29.676932308414127, -51.10047742419449";

        return $"destinations={destinations}&origins={origins}&key={googleMapsToken}";
    }

    private static string BuildQueryParams(Address address)
    {
        var googleMapsToken = Environment.GetEnvironmentVariable("GOOGLE_MAPS_TOKEN");

        if (string.IsNullOrEmpty(googleMapsToken))
        {
            throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = GoogleErrorMessages.GEO_CODE });
        }

        var addressParam = $"{address.StreetNumber}+{address.Street}+{address.District},+{address.City},+{address.State},+Brasil";

        return $"address={addressParam}&key={googleMapsToken}";
    }
}
