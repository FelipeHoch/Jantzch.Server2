using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Events.Constants;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Users.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;

namespace Jantzch.Server2;

public class CreateEventToManyUsersCommandHandler : IRequestHandler<CreateEventToManyUsersCommand, List<Event>>
{
    private readonly IEventRepository _eventRepository;

    private readonly IEventTypeRepository _eventTypeRepository;

    private readonly IUserRepository _userRepository;

    private readonly IMediator _mediator;

    public CreateEventToManyUsersCommandHandler(IEventRepository eventRepository, IUserRepository userRepository, IEventTypeRepository eventTypeRepository, IMediator mediator)
    {
        _eventRepository = eventRepository;

        _userRepository = userRepository;

        _eventTypeRepository = eventTypeRepository;

        _mediator = mediator;
    }

    public async Task<List<Event>> Handle(CreateEventToManyUsersCommand request, CancellationToken cancellationToken)
    {
        var events = new List<Event>();

        foreach (var user in request.Users)
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

            var userToAssociate = await _userRepository.GetByIdAsync(new ObjectId(user.Id), cancellationToken);

            if (userToAssociate is null)
            {
                throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = UserErrorMessages.NOT_FOUND });
            }

            @event.AssociateUser(user);

            await _eventRepository.AddAsync(@event, cancellationToken);

            events.Add(@event);

            if (request.NotifyUser)
            {
                await _mediator.Publish(new ManyEventsCreatedNotification(events), cancellationToken);
            }
        }

        return events;
    }
}
