using Jantzch.Server2.Domain.Entities.Users;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jantzch.Server2.Domain.Entities.Events; 

public class Event
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime EndDate { get; set; } = DateTime.UtcNow;

    public EventType EventType { get; private set; }

    public UserSimple User { get; private set; }

    public bool NotifyUser { get; set; } = false;

    public void AssociateUser(UserSimple user)
    {
        User = user;
    }

    public void DefineEventType(EventType eventType)
    {
        EventType = eventType;
    }

    public void Edit(string name, string description, DateTime startDate, DateTime endDate, EventType eventType, bool notifyUser, UserSimple user)
    {      
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        EventType = eventType;
        NotifyUser = notifyUser;
        User = user;
    }
}
