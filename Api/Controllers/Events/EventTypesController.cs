using Jantzch.Server2.Application.Events;
using Jantzch.Server2.Application.Events.CreateEventType;
using Jantzch.Server2.Application.Events.DeleteEventType;
using Jantzch.Server2.Application.Events.EditEventType;
using Jantzch.Server2.Application.Events.GetEventTypes;
using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.Events;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "admin,supervisor")]
public class EventTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<EventTypeResponse>>> GetAsync([FromQuery] ResourceParameters parameters, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new EventTypesQuery(parameters), cancellationToken);

        return Ok(response);
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<EventTypeResponse>> GetByIdAsync(string id, CancellationToken cancellationToken)
    //{
    //    var response = await _mediator.Send(new GetEventTypeQuery(id), cancellationToken);
    //    return Ok(response);
    //}

    [HttpPost]
    public async Task<ActionResult<EventTypeResponse>> CreateAsync([FromBody] CreateEventTypeCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EventTypeResponse>> UpdateAsync(string id, [FromBody] EditEventTypeCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new EditEventTypeCommand.Command(id, command), cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteEventTypeCommand(id), cancellationToken);

        return NoContent();
    }
}
