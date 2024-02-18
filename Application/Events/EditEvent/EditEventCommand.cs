using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Users;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MediatR;

namespace Jantzch.Server2.Application.Events.EditEvent;

public class EditEventCommand
{
    public string Name { get; set; }

    public string Description { get; set; }
    
    public DateTime StartDate { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime EndDate { get; set; }

    public EventType EventType { get; set; }

    public UserSimple User { get; set; }

    public bool NotifyUser { get; set; }

    public record Command(string Id, EditEventCommand EditEventCommand) : IRequest<Event>;
}
