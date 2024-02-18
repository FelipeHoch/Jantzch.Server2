using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Events.Constants;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Users.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;

namespace Jantzch.Server2.Application.Events.EditEvent;

public class EditEventCommandHandler : IRequestHandler<EditEventCommand.Command, Event>
{
    private readonly IEventRepository _eventRepository;

    private readonly IEventTypeRepository _eventTypeRepository;

    private readonly IUserRepository _userRepository;

    public EditEventCommandHandler(
        IEventRepository eventRepository,
        IEventTypeRepository eventTypeRepository,
        IUserRepository userRepository)
    {
        _eventRepository = eventRepository;
        _eventTypeRepository = eventTypeRepository;
        _userRepository = userRepository;
    }

    public async Task<Event> Handle(EditEventCommand.Command request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        var eventEdited = request.EditEventCommand; 
        
        if (@event is null)
        {
            throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = EventErrorMessages.NOT_FOUND });
        }

        var eventType = await _eventTypeRepository.GetByIdAsync(eventEdited.EventType.Id, cancellationToken);

        if (eventType is null)
        {
            throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = EventTypeErrorMessages.NOT_FOUND });
        }

        var user = await _userRepository.GetByIdAsync(new ObjectId(eventEdited.User.Id), cancellationToken);

        if (user is null)
        {
            throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = UserErrorMessages.NOT_FOUND });
        }

        @event.Edit(
            eventEdited.Name,
            eventEdited.Description, 
            eventEdited.StartDate,
            eventEdited.EndDate, 
            eventEdited.EventType, 
            eventEdited.NotifyUser,
            eventEdited.User
        );

        await _eventRepository.UpdateAsync(@event, cancellationToken);

        return @event;
    }
}
