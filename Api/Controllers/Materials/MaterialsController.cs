using Jantzch.Server2.Application.Materials;
using Jantzch.Server2.Application.Materials.CreateMaterial;
using Jantzch.Server2.Application.Materials.DeleteMaterial;
using Jantzch.Server2.Application.Materials.GetMaterial;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Features.Materials;
using Jantzch.Server2.Features.Materials.EditMaterial;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net;

namespace Jantzch.Server2.Api.Controllers.Materials;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(List<MaterialDTO>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> List([FromQuery] MaterialsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new MaterialsQuery(parameters), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MaterialDTO), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string id, [FromQuery] string? fields, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new MaterialQuery(id, fields), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMaterialCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public Task<Material> Edit(string id, [FromBody] EditMaterialCommand command, CancellationToken cancellationToken)
    {
        return _mediator.Send(new EditMaterialCommand.Command(command, id), cancellationToken);
    }

    [HttpDelete("{id}")]
    public Task Delete(string id, CancellationToken cancellationToken)
    {
        return _mediator.Send(new DeleteMaterialCommand.Command(id), cancellationToken);
    }
}
