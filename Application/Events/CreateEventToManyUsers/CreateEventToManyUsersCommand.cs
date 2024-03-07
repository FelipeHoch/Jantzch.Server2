using Amazon.Runtime.Internal;
using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;

namespace Jantzch.Server2;

public class CreateEventToManyUsersCommand : IRequest<List<Event>>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public EventType EventType { get; set; }

    public List<UserSimple> Users { get; set; }

    public bool NotifyUser { get; set; }
}
