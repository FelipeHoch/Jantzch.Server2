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

    [BsonElement("route")]
    public Route? Route { get; set; }

    [BsonElement("address")]
    public Address? Address { get; set; }

    [BsonElement("location")]
    public Location? Location { get; set; }
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


