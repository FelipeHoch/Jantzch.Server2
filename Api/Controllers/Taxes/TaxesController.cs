using Jantzch.Server2.Application.Taxes;
using Jantzch.Server2.Application.Taxes.GetTax;
using Jantzch.Server2.Application.Taxes.GetTaxes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Jantzch.Server2.Api.Controllers.Taxes;

[ApiController]
[Route("api/[controller]")]
public class TaxesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(List<TaxResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> List([FromQuery] TaxesResourceParameters parameters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TaxesQuery(parameters), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaxResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string id, [FromQuery] string? fields, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TaxQuery(id, fields), cancellationToken);

        return Ok(result);
    }

    //[HttpPost]
    //[ProducesResponseType(typeof(Tax), (int)HttpStatusCode.Created)]
    //public async Task<IActionResult> Create([FromBody] CreateTaxCommand command, CancellationToken cancellationToken)
    //{
    //    var result = await _mediator.Send(command, cancellationToken);

    //    return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    //}

    //[HttpPut("{id}")]
    //[ProducesResponseType(typeof(Tax), (int)HttpStatusCode.Created)]
    //public async Task<IActionResult> Edit(string id, [FromBody] EditTaxCommand command, CancellationToken cancellationToken)
    //{
    //    var result = await _mediator.Send(new EditTaxCommand.Command(command, id), cancellationToken);

    //    return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    //}

    //[HttpDelete("{id}")]
    //[ProducesResponseType((int)HttpStatusCode.NoContent)]
    //public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    //{
    //    await _mediator.Send(new DeleteTaxCommand(id), cancellationToken);

    //    return NoContent();
    //}
}
