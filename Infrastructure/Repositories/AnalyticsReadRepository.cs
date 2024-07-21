using Jantzch.Server2.Application.Abstractions.Repositories;
using Jantzch.Server2.Application.Deals.Analytics.DTOs;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class AnalyticsReadRepository(IMongoDatabase database) : IAnalyticsReadRepository
{
    private readonly IMongoCollection<Deal> _deals = database.GetCollection<Deal>("deals");

    public async Task<List<DealAnalyticByInstallationType>> GetDealAnalyticByInstallationTypeAsync(DateTime initial, CancellationToken cancellationToken)
    {
        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument
            {
                { "dealConfirmedAt", new BsonDocument("$gte", initial) }
            }),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$instalationType" },
                { "totalRevenue", new BsonDocument("$sum", "$value") }
            }),
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "installation", "$_id" },
                { "totalRevenue", 1 }
            }),
            new BsonDocument("$sort", new BsonDocument("totalRevenue", -1))
        };

        var result = await _deals.Aggregate<DealAnalyticByInstallationType>(pipeline, cancellationToken: cancellationToken).ToListAsync(cancellationToken);

        return result;
    }

    public async Task<List<DealAnalyticByCity>> GetDealAnalyticByCityAsync(DateTime initial, CancellationToken cancellationToken)
    {
        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument
            {
                { "dealConfirmedAt", new BsonDocument("$gte", initial) }
            }),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$address.city" },
                { "totalRevenue", new BsonDocument("$sum", "$value") }
            }),
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "city", "$_id" },
                { "totalRevenue", 1 }
            }),
            new BsonDocument("$sort", new BsonDocument("totalRevenue", -1))
        };

        var result = await _deals.Aggregate<DealAnalyticByCity>(pipeline, cancellationToken: cancellationToken).ToListAsync(cancellationToken);

        return result;
    }

    public async Task<List<DealAnalyticByMonth>> GetDealAnalyticByMonthAsync(DateTime initial, CancellationToken cancellationToken)
    {
        var pipeline = new[]
        {
            // Fase $match
            new BsonDocument("$match", new BsonDocument
            {
                { "dealConfirmedAt", new BsonDocument("$gte", initial) }
            }),

            // Fase $group
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", new BsonDocument
                    {
                        { "year", new BsonDocument("$year", "$dealConfirmedAt") },
                        { "month", new BsonDocument("$month", "$dealConfirmedAt") }
                    }
                },
                { "totalRevenue", new BsonDocument("$sum", "$value") },
                { "date", new BsonDocument("$max", "$dealConfirmedAt") },
                { "count", new BsonDocument("$sum", 1) }
            }),

            // Fase $project
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "month", new BsonDocument
                    {
                        { "$concat", new BsonArray
                            {
                                new BsonDocument("$toString", "$_id.month"),
                                "/",
                                new BsonDocument("$toString", "$_id.year")
                            }
                        }
                    }
                },
                { "date", 1 },
                { "totalRevenue", 1 },
                { "count", 1 }
            }),

            // Fase $sort
            new BsonDocument("$sort", new BsonDocument
            {
                { "date", 1 }
            })
        };

        var result = await _deals.Aggregate<DealAnalyticByMonth>(pipeline, cancellationToken: cancellationToken).ToListAsync(cancellationToken);

        return result;
    }
}
