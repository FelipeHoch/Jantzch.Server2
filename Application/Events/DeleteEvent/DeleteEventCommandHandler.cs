using System.Net;
using Jantzch.Server2.Domain.Entities.Events;
using Jantzch.Server2.Domain.Entities.Events.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IEventRepository _eventRepository;

    public DeleteEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);

        if (@event is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = EventErrorMessages.NOT_FOUND });
        }

        await _eventRepository.DeleteAsync(@event, cancellationToken);
    }
}
