using Jantzch.Server2.Domain.Entities.Clients.Enums;
using Jantzch.Server2.Infrastructure.Google.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Clients;

[BsonIgnoreExtraElements]
public class Client
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("cnpj")]
    public string? Cnpj { get; set; }

    [BsonElement("cpf")]
    public string? Cpf { get; set; }  

    [BsonElement("phoneNumber")]
    public string PhoneNumber { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement]
    public Address? Address { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("location")]
    public Location? Location { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("route")]
    public Route? Route { get; set; }

    [BsonElement("types")]
    public List<ClientType> Types { get; set; } = [];

    [BsonElement("localizations")]
    public List<Localization> Localizations { get; set; } = [];

    public void AddLocalization(Location location, Address address, Route route, string description, bool? isPrimary)
    {
        var localization = new Localization
        {
            Location = location,
            Address = address,
            Route = route,
            Description = description,
            IsPrimary = isPrimary ?? false
        };

        if (isPrimary ?? false)
        {
            Localizations.ForEach(l => l.IsPrimary = false);
        }

        Localizations.Add(localization);
    }

    public void RemoveLocalization(string Id)
    {
        var localization = Localizations.Find(l => l.Id == Id);

        if (localization is null)
        {
            return;
        }

        Localizations.Remove(localization);

        SetPrimaryIfHavent();
    }

    public void UpdateLocalization(string id, Location location, Address address, Route route, string description, bool? isPrimary)
    {
        var localization = Localizations.Find(l => l.Id == id);

        if (localization is null)
        {
            return;
        }

        if (isPrimary ?? false)
        {
            Localizations.ForEach(l => l.IsPrimary = false);
        }

        localization.Address = address;
        localization.Route = route;
        localization.Description = description;
        localization.IsPrimary = isPrimary ?? false;
        localization.Location = location;        
    }

    private void SetPrimaryIfHavent()
    {
        var hasPrimary = Localizations.Exists(l => l.IsPrimary);

        if (!hasPrimary && Localizations.Count > 0)
        {
            Localizations[0].IsPrimary = true;
        }
    }
}

public class Localization {
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("location")]
    public Location? Location { get; set; }

    [BsonElement("address")]
    public Address? Address { get; set; }

    [BsonElement("route")]
    public Route? Route { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("isPrimary")]
    public bool IsPrimary { get; set; }
}

public class Address
{
    [BsonElement("street")]
    public string Street { get; set; }

    [BsonElement("streetNumber")]
    public int StreetNumber { get; set; }

    [BsonElement("district")]
    public string District { get; set; }

    [BsonElement("city")]
    public string City { get; set; }

    [BsonElement("state")]
    public string State { get; set; }
}

public class Location
{
    [BsonElement("location")]
    public double Latitude { get; set; }

    [BsonElement("longitude")]
    public double Longitude { get; set; }
}

public class Route
{
    [BsonElement("distance")]
    public Measurement Distance { get; set; }

    [BsonElement("duration")]
    public Measurement Duration { get; set; }
}


