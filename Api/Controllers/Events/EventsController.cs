using Jantzch.Server2.Application.Events;
using Jantzch.Server2.Application.Events.CreateEvent;
using Jantzch.Server2.Application.Events.EditEvent;
using Jantzch.Server2.Application.Events.GetEvents;
using Jantzch.Server2.Domain.Entities.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.Events;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<EventResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEvents([FromQuery] EventResourceParameters parameters, CancellationToken cancellationToken)
    {
        var events = await _mediator.Send(new EventsQuery(parameters), cancellationToken);

        return Ok(events);
    }

    [HttpGet("user")]
    [ProducesResponseType(typeof(List<EventResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEventsByUser([FromQuery] EventResourceParameters parameters, CancellationToken cancellationToken)
    {
        var events = await _mediator.Send(new EventsByUserQuery(parameters), cancellationToken);

        return Ok(events);
    }

    //[HttpGet("{id}")]
    //[ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetEvent(string id, [FromQuery] string? fields, CancellationToken cancellationToken)
    //{
    //    var @event = await _mediator.Send(new EventQuery(id, fields), cancellationToken);

    //    return Ok(@event);
    //}

    [HttpPost]
    [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command, CancellationToken cancellationToken)
    {
        var @event = await _mediator.Send(command, cancellationToken);

        return Ok(@event);
    }

    [HttpPost("many")]
    [ProducesResponseType(typeof(List<Event>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEventToManyUsers([FromBody] CreateEventToManyUsersCommand command, CancellationToken cancellationToken)
    {
        var events = await _mediator.Send(command, cancellationToken);

        return Ok(events);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditEvent(string id, [FromBody] EditEventCommand model, CancellationToken cancellationToken)
    {
        var @event = await _mediator.Send(new EditEventCommand.Command(id, model), cancellationToken);

        return Ok(@event);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEvent(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteEventCommand(id), cancellationToken);

        return NoContent();
    }
}
