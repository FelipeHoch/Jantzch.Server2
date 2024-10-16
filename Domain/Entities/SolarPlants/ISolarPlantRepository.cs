namespace Jantzch.Server2.Domain.Entities.SolarPlants;

public interface ISolarPlantRepository
{
    Task<IEnumerable<SolarPlant>> GetAllAsync();

    Task<SolarPlant> GetByIdAsync(string id);

    Task<SolarPlant> CreateAsync(SolarPlant solarPlant);

    Task<SolarPlant> UpdateAsync(SolarPlant solarPlant);

    Task<SolarConsumer> CreateConsumerAsync(string solarPlantId, SolarConsumer solarConsumer);

    Task<SolarConsumer> UpdateConsumerAsync(string solarPlantId, SolarConsumer solarConsumer);

    Task DeleteConsumerAsync(string solarPlantId, string consumerId);           

    Task DeleteAsync(string id);
}
