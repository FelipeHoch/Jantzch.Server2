using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Jantzch.Server2.Infraestructure;

public class MongoRepository : IMongoRepository
{
    private readonly IMongoClient _mongoClient;

    public IMongoClient Client => _mongoClient;

    private readonly JantzchContext _context;

    public JantzchContext Context => _context;

    public MongoRepository(MongoRepositoryOptions mongoRepositoryOptions)
    {
        var conf = MongoClientSettings.FromConnectionString(mongoRepositoryOptions.ConnectionString);

        conf.ApplicationName = mongoRepositoryOptions.ClientName;

        _mongoClient = new MongoClient(conf);

        var dbContextOptions = new DbContextOptionsBuilder<JantzchContext>()
            .UseMongoDB(mongoRepositoryOptions.ConnectionString, "jantzch");
        
        _context = new JantzchContext(dbContextOptions.Options);
    }
}
