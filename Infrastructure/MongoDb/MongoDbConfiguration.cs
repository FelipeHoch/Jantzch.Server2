using Domain.Entities.Orders.Enums;
using Jantzch.Server2.Domain.Entities.Clients.Enums;
using Jantzch.Server2.Infraestructure;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Jantzch.Server2.Infrastructure.MongoDb;

public static class MongoDbConfiguration
{
    public static void ConfigureMongoDB(this IServiceCollection services)
    {
        var mongoClientSettings = MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable("MONGODB_URI") ?? "mongodb://localhost:27017");
        mongoClientSettings.ApplicationName = "JANTZCH";

        var mongoClient = new MongoClient(mongoClientSettings);
        var database = mongoClient.GetDatabase("jantzch");

        RegisterConventions();

        services.AddSingleton(database);

        services.AddDbContext<JantzchContext>(opt => opt.UseMongoDB(mongoClient, database.DatabaseNamespace.DatabaseName));
    }

    private static void RegisterConventions()
    {
        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCaseElementNameConvention", conventionPack, t => true);

        ConventionRegistry.Register("IgnoreExtraElements", new ConventionPack { new IgnoreExtraElementsConvention(true) }, _ => true);

        ConventionRegistry.Register("EnumStringRepresentationConvention", new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, t => t.IsEnum);

        BsonSerializer.RegisterSerializer(typeof(OrderType), new EnumSerializer<OrderType>(BsonType.String));

        BsonSerializer.RegisterSerializer(typeof(ClientType), new EnumSerializer<ClientType>(BsonType.String));
    }
}
