using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Materials.CreateMaterial;
using Jantzch.Server2.Application.Materials.DeleteMaterial;
using Jantzch.Server2.Application.Materials.GetMaterials;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Features.Materials;
using Jantzch.Server2.Features.Materials.EditMaterial;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.Materials;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController(IMediator mediator)
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public Task<PagedList<MaterialDTO>> List([FromQuery] MaterialsResourceParameters parameters, CancellationToken cancellationToken)
    {
        return _mediator.Send(new MaterialsQuery(parameters), cancellationToken);
    }

    [HttpPost]
    public Task<Material> Create([FromBody] CreateMaterialCommand command, CancellationToken cancellationToken)
    {
        return _mediator.Send(command, cancellationToken);
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
