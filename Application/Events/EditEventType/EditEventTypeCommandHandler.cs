using Jantzch.Server2.Application.Events.Notifications;
using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Events.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.Events.EditEventType;

public class EditEventTypeCommandHandler : IRequestHandler<EditEventTypeCommand.Command, EventType>
{
    private readonly IEventTypeRepository _eventTypeRepository;

    private readonly IMediator _mediator;

    public EditEventTypeCommandHandler(
        IEventTypeRepository eventTypeRepository, 
        IMediator mediator)
    {
        _eventTypeRepository = eventTypeRepository;
        _mediator = mediator;
    }

    public async Task<EventType> Handle(EditEventTypeCommand.Command request, CancellationToken cancellationToken)
    {
        var eventType = await _eventTypeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (eventType is null)
        {
            throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = EventTypeErrorMessages.NOT_FOUND });
        }

        eventType.Name = request.Model.Name;
        eventType.HexColor = request.Model.HexColor;

        await _eventTypeRepository.UpdateAsync(eventType, cancellationToken);

        await _mediator.Publish(new EventTypeEditedNotification(eventType), cancellationToken);

        return eventType;
    }
}
