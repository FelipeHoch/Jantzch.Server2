using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Features.Materials;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController
{
    private readonly IMediator _mediator;

    public MaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<MaterialsEnvelope> List(CancellationToken cancellationToken)
    {
        return _mediator.Send(new List.Query(), cancellationToken);
    }

    [HttpPost]
    public Task<MaterialEnvelope> Create([FromBody] Create.Command command, CancellationToken cancellationToken)
    {
        return _mediator.Send(command, cancellationToken);
    }

    [HttpPut("{id}")]
    public Task<MaterialEnvelope> Edit(string id, [FromBody] Edit.Model model, CancellationToken cancellationToken)
    {

        return _mediator.Send(new Edit.Command(model, id), cancellationToken);
    }

    [HttpDelete("{id}")]
    public Task Delete(string id, CancellationToken cancellationToken)
    {
        return _mediator.Send(new Delete.Command(id), cancellationToken);
    }
}
