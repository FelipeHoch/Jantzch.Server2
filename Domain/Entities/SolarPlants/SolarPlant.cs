using Jantzch.Server2.Domain.Entities.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.SolarPlants;

[BsonIgnoreExtraElements]
public class SolarPlant
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Capacity { get; set; } = 0;

    public bool IsActived { get; set; } = true;

    public List<SolarConsumer> SolarConsumers { get; set; } = [];

    public UserSimple? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastUpdateAt { get; set; } = null;

    public bool IsAvailableReceiveConsumer(SolarConsumer consumer)
    {
        var actualConsumption = SolarConsumers.Sum(x => x.Consumption);

        return actualConsumption + consumer.Consumption <= Capacity;
    }

    public bool IsCapacityValid(int capacity)
    {
        if (capacity <= 0)
        {
            return false;
        }

        var actualConsumption = SolarConsumers.Sum(x => x.Consumption);

        return actualConsumption <= capacity;
    }
}
