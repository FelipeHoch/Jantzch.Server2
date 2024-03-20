using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PotentialOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PotentialOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PotentialOrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPotentialOrders([FromQuery] PotentialOrderResourceParameters parameters, CancellationToken cancellationToken)
    {
        var clients = await _mediator.Send(new PotentialOrdersQuery(parameters), cancellationToken);

        return Ok(clients);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PotentialOrder), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePotentialOrder([FromBody] CreatePotentialOrderCommand command, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(command, cancellationToken);

        return Ok(client);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PotentialOrder), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditPotentialOrder(string id, [FromBody] EditPotentialOrderCommand command, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new EditPotentialOrderCommand.Command(id, command), cancellationToken);

        return Ok(client);
    }

    [HttpPut("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CancelPotentialOrder(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelPotentialOrderCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpPut("{id}/convert")]
    [ProducesResponseType(typeof(Order) ,StatusCodes.Status200OK)]
    public async Task<IActionResult> ConvertPotentialOrder(string id, CancellationToken cancellationToken)
    {
        var order = await _mediator.Send(new ConvertPotentialOrderCommand(id), cancellationToken);

        return Ok(order);
    }
}
