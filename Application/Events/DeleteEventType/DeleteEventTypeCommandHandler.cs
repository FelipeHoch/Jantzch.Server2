using System.Net;
using Jantzch.Server2.Application.Events.Notifications;
using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Events.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.Events.DeleteEventType;

public class DeleteEventTypeCommandHandler : IRequestHandler<DeleteEventTypeCommand>
{
    private readonly IEventTypeRepository _eventTypeRepository;

    private readonly IMediator _mediator;

    public DeleteEventTypeCommandHandler(IEventTypeRepository eventTypeRepository, IMediator mediator)
    {
        _eventTypeRepository = eventTypeRepository;

        _mediator = mediator;
    }

    public async Task Handle(DeleteEventTypeCommand request, CancellationToken cancellationToken)
    {
        var eventType = await _eventTypeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (eventType is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = EventTypeErrorMessages.NOT_FOUND });
        }

        await _eventTypeRepository.DeleteAsync(eventType, cancellationToken);

        await _mediator.Publish(new EventTypeDeletedNotification(eventType), cancellationToken);
    }
}