using Jantzch.Server2.Application.OrderReports;
using Jantzch.Server2.Application.OrderReports.GetOrderReports;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.OrderReports;

[Route("api/clients/{clientId}/report/")]
[ApiController]
public class OrderReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderReportResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderReports(string clientId, [FromQuery] OrderReportResourceParameters parameters, CancellationToken cancellationToken)
    {
        var orderReports = await _mediator.Send(new OrderReportsQuery(clientId, parameters), cancellationToken);

        return Ok(orderReports);
    }
}
