﻿using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Application.Orders.CreateBreak;
using Jantzch.Server2.Application.Orders.CreateOrder;
using Jantzch.Server2.Application.Orders.DeleteOrder;
using Jantzch.Server2.Application.Orders.EditOrder;
using Jantzch.Server2.Application.Orders.GetOrder;
using Jantzch.Server2.Application.Orders.GetOrders;
using Jantzch.Server2.Application.Orders.GetOrderImages;
using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.Orders;

[ApiController]
[Authorize]
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

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditOrder(string id, [FromBody] JsonPatchDocument<Order> model, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new EditOrderCommand.Command(model, id), cancellationToken);

        return Ok(client);
    }

    [HttpPost("{id}/break")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateBreak(string id, [FromBody] CreateBreakCommand command, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new CreateBreakCommand.Command(id, command.Descriptive), cancellationToken);

        return Ok(client);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOrder(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOrderCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpGet("{id}/images")]
    [ProducesResponseType(typeof(OrderImagesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderImages(string id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new OrderImagesQuery(id), cancellationToken);
        return Ok(response);
    }
}