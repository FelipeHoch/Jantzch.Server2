using Jantzch.Server2.Application.Clients;
using Jantzch.Server2.Application.Clients.CreateClient;
using Jantzch.Server2.Application.Clients.DeleteAddress;
using Jantzch.Server2.Application.Clients.DeleteClient;
using Jantzch.Server2.Application.Clients.EditAddress;
using Jantzch.Server2.Application.Clients.EditClient;
using Jantzch.Server2.Application.Clients.GetClients;
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

    private readonly IClientsRepository clientsRepository;

    public ClientsController(IMediator mediator, IClientsRepository clientsRepository)
    {
        _mediator = mediator;
        this.clientsRepository = clientsRepository;
    }

    [HttpGet("script")]
    public async Task<IActionResult> GetScript(CancellationToken cancellationToken)
    {
        await clientsRepository.ScriptToUpdateAddressToLocalization();

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ClientResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClients([FromQuery] ClientsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var clients = await _mediator.Send(new ClientsQuery(parameters), cancellationToken);

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
     
    [HttpDelete("{id}/address/{addressId}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteClientAddress(string id, string addressId, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new DeleteAddress.Command(id, addressId), cancellationToken);

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