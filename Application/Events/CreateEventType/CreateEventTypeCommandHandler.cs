using Jantzch.Server2.Domain.Entities.Events;
using MediatR;

namespace Jantzch.Server2.Application.Events.CreateEventType;

public class CreateEventTypeCommandHandler : IRequestHandler<CreateEventTypeCommand, EventType>
{
    private readonly IEventTypeRepository _eventTypeRepository;

    public CreateEventTypeCommandHandler(IEventTypeRepository eventTypeRepository)
    {
        _eventTypeRepository = eventTypeRepository;
    }

    public async Task<EventType> Handle(CreateEventTypeCommand request, CancellationToken cancellationToken)
    {
        var eventType = new EventType
        {
            Name = request.Name,
            HexColor = request.HexColor
        };

        await _eventTypeRepository.AddAsync(eventType, cancellationToken);

        return eventType;
    } 
}
