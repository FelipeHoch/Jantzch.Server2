using MongoDB.Driver;

namespace Jantzch.Server2.Infraestructure;

public interface IMongoRepository
{
    IMongoClient Client { get; }

    JantzchContext Context { get; }
}
