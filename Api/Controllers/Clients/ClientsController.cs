using Jantzch.Server2.Application.Clients;
using Jantzch.Server2.Application.Clients.CreateClient;
using Jantzch.Server2.Application.Clients.DeleteClient;
using Jantzch.Server2.Application.Clients.EditAddress;
using Jantzch.Server2.Application.Clients.EditClient;
using Jantzch.Server2.Application.Clients.GetClients;
using Jantzch.Server2.Application.Clients.GetClientsInformation;
using Jantzch.Server2.Domain.Entities.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.Clients;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ClientResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClients([FromQuery] ClientsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var clients = await _mediator.Send(new ClientsQuery(parameters), cancellationToken);

        return Ok(clients);
    }

    [HttpGet("information")]
    [ProducesResponseType(typeof(List<ClientInformationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientsInformation([FromQuery] ClientsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var clients = await _mediator.Send(new ClientsInformationQuery(parameters), cancellationToken);

        return Ok(clients);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(Client), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(command, cancellationToken);

        return Ok(client);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditClient(string id, [FromBody] EditClientCommand command, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new EditClientCommand.Command(command, id), cancellationToken);

        return Ok(client);
    }

    [HttpPut("{id}/address")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditClientAddress(string id, [FromBody] EditAddressCommand command, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new EditAddressCommand.Command(command, id), cancellationToken);

        return Ok(client);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteClient(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteClientCommand(id), cancellationToken);

        return NoContent();
    }
}