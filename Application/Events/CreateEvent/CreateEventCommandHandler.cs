using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Events.Constants;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Users.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;

namespace Jantzch.Server2.Application.Events.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Event>
{
    private readonly IEventRepository _eventRepository;

    private readonly IEventTypeRepository _eventTypeRepository;

    private readonly IUserRepository _userRepository;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUserRepository userRepository, IEventTypeRepository eventTypeRepository)
    {
        _eventRepository = eventRepository;

        _userRepository = userRepository;

        _eventTypeRepository = eventTypeRepository;
    }

    public async Task<Event> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = new Event
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NotifyUser = request.NotifyUser
        };

        var eventType = await _eventTypeRepository.GetByIdAsync(request.EventType.Id, cancellationToken);

        if (eventType is null)
        {
            throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = EventTypeErrorMessages.NOT_FOUND });
        }

        @event.DefineEventType(request.EventType);

        var userToAssociate = await _userRepository.GetByIdAsync(new ObjectId(request.User.Id), cancellationToken);

        if (userToAssociate is null)
        {
            throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = UserErrorMessages.NOT_FOUND });
        }

        @event.AssociateUser(request.User);

        await _eventRepository.AddAsync(@event, cancellationToken);

        return @event;
    }
}
