using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Application.Orders.CreateOrder;
using Jantzch.Server2.Application.Orders.GetOrder;
using Jantzch.Server2.Application.Orders.GetOrders;
using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.Orders;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders([FromQuery] OrderResourceParameters parameters, CancellationToken cancellationToken)
    {
        var clients = await _mediator.Send(new OrdersQuery(parameters), cancellationToken);

        return Ok(clients);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrder(string id, [FromQuery] string? fields, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new OrderQuery(id, fields), cancellationToken);

        return Ok(client);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(command, cancellationToken);

        return Ok(client);
    }

    //[HttpPut("{id}")]
    //[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    //public async Task<IActionResult> EditOrder(string id, [FromBody] EditOrderCommand command, CancellationToken cancellationToken)
    //{
    //    var client = await _mediator.Send(new EditOrderCommand.Command(command, id), cancellationToken);

    //    return Ok(client);
    //}

    //[HttpDelete("{id}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //public async Task<IActionResult> DeleteOrder(string id, CancellationToken cancellationToken)
    //{
    //    await _mediator.Send(new DeleteOrderCommand(id), cancellationToken);

    //    return NoContent();
    //}
}