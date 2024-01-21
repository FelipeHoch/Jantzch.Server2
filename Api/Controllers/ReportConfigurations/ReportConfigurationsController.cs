using Jantzch.Server2.Application.ReportConfigurations;
using Jantzch.Server2.Application.ReportConfigurations.CreateConfiguration;
using Jantzch.Server2.Application.ReportConfigurations.DeleteReportConfiguration;
using Jantzch.Server2.Application.ReportConfigurations.EditConfiguration;
using Jantzch.Server2.Application.ReportConfigurations.GetReportConfiguration;
using Jantzch.Server2.Application.ReportConfigurations.GetReportConfigurations;
using Jantzch.Server2.Application.Shared;
using Jantzch.Server2.Application.Taxes;
using Jantzch.Server2.Application.Taxes.CreateTax;
using Jantzch.Server2.Application.Taxes.DeleteTax;
using Jantzch.Server2.Application.Taxes.EditTax;
using Jantzch.Server2.Application.Taxes.GetTax;
using Jantzch.Server2.Application.Taxes.GetTaxes;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Domain.Entities.Taxes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Jantzch.Server2.Api.Controllers.ReportConfigurations;

[ApiController]
[Route("api/[controller]")]
public class ReportConfigurationController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(List<ReportConfigurationResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> List([FromQuery] ResourceParameters parameters, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ConfigurationsQuery(parameters), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{key}")]
    [ProducesResponseType(typeof(ReportConfigurationResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string key, [FromQuery] string? fields, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ConfigurationQuery(key, fields), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReportConfiguration), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] CreateConfigurationCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(typeof(ReportConfiguration), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Edit(string key, [FromBody] EditConfigurationCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new EditConfigurationCommand.Command(command, key), cancellationToken);

        return CreatedAtAction(nameof(Get), new { key = result.ReportKey }, result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteConfigurationCommand(id), cancellationToken);

        return NoContent();
    }
}
