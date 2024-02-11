using Jantzch.Server2.Application.Materials;
using Jantzch.Server2.Application.Materials.CreateMaterial;
using Jantzch.Server2.Application.Materials.DeleteMaterial;
using Jantzch.Server2.Application.Materials.GetMaterial;
using Jantzch.Server2.Features.Materials;
using Jantzch.Server2.Features.Materials.EditMaterial;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Jantzch.Server2.Api.Controllers.Materials;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MaterialsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(List<MaterialResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> List([FromQuery] MaterialsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new MaterialsQuery(parameters), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MaterialResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string id, [FromQuery] string? fields, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new MaterialQuery(id, fields), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "admin,supervisor")]
    [ProducesResponseType(typeof(MaterialResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] CreateMaterialCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,supervisor")]
    [ProducesResponseType(typeof(MaterialResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Edit(string id, [FromBody] EditMaterialCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new EditMaterialCommand.Command(command, id), cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,supervisor")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteMaterialCommand.Command(id), cancellationToken);

        return NoContent();
    }
}
