using Jantzch.Server2.Domain.Entities.SolarPlants;
using MongoDB.Driver;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class SolarPlantRepository(
    IMongoDatabase database
    ) : ISolarPlantRepository
{
    private readonly IMongoCollection<SolarPlant> _solarPlants = database.GetCollection<SolarPlant>("solarPlants");

    public async Task<IEnumerable<SolarPlant>> GetAllAsync()
    {
        return await _solarPlants.Find(_ => true).ToListAsync();
    }

    public async Task<SolarPlant> GetByIdAsync(string id)
    {
        return await _solarPlants.Find(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task<SolarPlant> CreateAsync(SolarPlant solarPlant)
    {
        await _solarPlants.InsertOneAsync(solarPlant);

        return solarPlant;
    }

    public async Task<SolarPlant> UpdateAsync(SolarPlant solarPlant)
    {
        await _solarPlants.ReplaceOneAsync(s => s.Id == solarPlant.Id, solarPlant);

        return solarPlant;
    }

    public async Task<SolarConsumer> CreateConsumerAsync(string solarPlantId, SolarConsumer solarConsumer)
    {
        var solarPlant = await GetByIdAsync(solarPlantId);

        solarPlant.SolarConsumers.Add(solarConsumer);

        await UpdateAsync(solarPlant);

        return solarConsumer;
    }

    public async Task<SolarConsumer> UpdateConsumerAsync(string solarPlantId, SolarConsumer solarConsumer)
    {
        var solarPlant = await GetByIdAsync(solarPlantId);

        var index = solarPlant.SolarConsumers.ToList().FindIndex(c => c.Id == solarConsumer.Id);

        solarPlant.SolarConsumers[index] = solarConsumer;

        await UpdateAsync(solarPlant);

        return solarConsumer;
    }

    public async Task DeleteConsumerAsync(string solarPlantId, string consumerId)
    {
        var solarPlant = await GetByIdAsync(solarPlantId);

        solarPlant.SolarConsumers = solarPlant.SolarConsumers.Where(c => c.Id != consumerId).ToList();

        await UpdateAsync(solarPlant);
    }

    public async Task DeleteAsync(string id)
    {
        await _solarPlants.DeleteOneAsync(s => s.Id == id);
    }
}
