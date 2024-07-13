using Jantzch.Server2.Application.Abstractions.Excel;
using Jantzch.Server2.Application.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Jantzch.Server2.Api.Controllers.Deals;

[Route("api/[controller]")]
[ApiController]
public class DealsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DealsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExpandoObject>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeals([FromQuery] DealsResourceParamenters parameters, CancellationToken cancellationToken)
    {
        var deals = await _mediator.Send(new ListDeals.Query(parameters), cancellationToken);

        return Ok(deals);
    }

    [HttpPost("import")]
    [ProducesResponseType(typeof(IEnumerable<DealResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDeal([FromForm] ImportDeals.Command file, CancellationToken cancellationToken)
    {
        var deal = await _mediator.Send(file, cancellationToken);

        return Ok(deal);
    }
}
