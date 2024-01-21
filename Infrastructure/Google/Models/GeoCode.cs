using System.Text.Json.Serialization;

public sealed class GeoCode
{
    public List<GeoCodeResult> Results { get; set; }

    public string Status { get; set; }
}

public sealed class GeoCodeResult
{
    [JsonPropertyName("address_components")]
    public List<AddressComponent> AddressComponents { get; set; }

    [JsonPropertyName("formatted_address")]
    public string FormattedAddress { get; set; }

    public Geometry Geometry { get; set; }

    [JsonPropertyName("place_id")]
    public string PlaceId { get; set; }

    public List<string> Types { get; set; }
}

public sealed class AddressComponent
{
    [JsonPropertyName("long_name")]
    public string LongName { get; set; }

    [JsonPropertyName("short_name")]
    public string ShortName { get; set; }

    public List<string> Types { get; set; }
}

public sealed class Geometry
{
    public Area Bounds { get; set; }

    public Coordinates Location { get; set; }

    [JsonPropertyName("location_type")]
    public string LocationType { get; set; }

    public Area Viewport { get; set; }
}

public sealed class Area
{
    public Coordinates Northeast { get; set; }

    public Coordinates Southwest { get; set; }
}

public sealed class Coordinates
{
    public double Lat { get; set; }

    public double Lng { get; set; }
} 