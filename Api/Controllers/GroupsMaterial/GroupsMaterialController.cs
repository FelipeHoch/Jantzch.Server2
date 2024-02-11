using Jantzch.Server2.Application.GroupsMaterial;
using Jantzch.Server2.Application.GroupsMaterial.CreateGroupMaterial;
using Jantzch.Server2.Application.GroupsMaterial.DeleteGroupMaterial;
using Jantzch.Server2.Application.GroupsMaterial.EditGroupMaterial;
using Jantzch.Server2.Application.GroupsMaterial.GetGroupMaterial;
using Jantzch.Server2.Application.GroupsMaterial.GetGroupsMaterial;
using Jantzch.Server2.Application.GroupsMaterial.GetGroupsWithMaterials;
using Jantzch.Server2.Application.Shared;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Jantzch.Server2.Api.Controllers.GroupsMaterial;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class GroupsMaterialController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(List<GroupMaterialResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> List([FromQuery] ResourceParameters parameters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GroupsMaterialQuery(parameters), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GroupMaterialResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string id, [FromQuery] string? fields, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GroupMaterialQuery(id, fields), cancellationToken);

        return Ok(result);
    }

    [HttpGet("withMaterials")]
    [ProducesResponseType(typeof(List<GroupMaterialResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetGroupsWithMaterials(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GroupsWithMaterialsQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "admin,supervisor")]
    [ProducesResponseType(typeof(GroupMaterial), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] CreateGroupMaterialCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin,supervisor")]
    [ProducesResponseType(typeof(GroupMaterial), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Edit(string id, [FromBody] EditGroupMaterialCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new EditGroupMaterialCommand.Command(command, id), cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,supervisor")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGroupMaterialCommand(id), cancellationToken);

        return NoContent();
    }
}

