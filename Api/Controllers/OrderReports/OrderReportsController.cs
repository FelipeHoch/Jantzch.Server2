using Jantzch.Server2.Application.OrderReports;
using Jantzch.Server2.Application.OrderReports.CreateManualReport;
using Jantzch.Server2.Application.OrderReports.CreateOrderReport;
using Jantzch.Server2.Application.OrderReports.DeleteOrderReport;
using Jantzch.Server2.Application.OrderReports.GetExportByOrder;
using Jantzch.Server2.Application.OrderReports.GetOrderReport;
using Jantzch.Server2.Application.OrderReports.GetOrderReports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.OrderReports;

[Route("api/clients/{clientId}/report/")]
[Authorize(Roles = "admin,supervisor")]
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

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderReportResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderReport(string clientId, string id, CancellationToken cancellationToken)
    {
        var orderReport = await _mediator.Send(new OrderReportQuery(id, clientId), cancellationToken);

        return Ok(orderReport);
    }

    [HttpGet("orders/{orderId}")]
    [ProducesResponseType(typeof(OrderReportResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderReportByOrderId([FromQuery] string taxesId, string clientId, string orderId, CancellationToken cancellationToken)
    {
        var orderReport = await _mediator.Send(new ReportByOrderQuery(clientId, orderId, taxesId), cancellationToken);

        return Ok(orderReport);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderReportResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrderReport(string clientId, [FromBody] CreateOrderReportCommand command, CancellationToken cancellationToken)
    {
        var orderReport = await _mediator.Send(new CreateOrderReportCommand.Command(command.OrdersId, command.TaxesId, clientId), cancellationToken);

        return Ok(orderReport);
    }

    [HttpPost("manual")]
    [ProducesResponseType(typeof(OrderReportResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateManualReport(string clientId, string? taxesId, [FromBody] List<CreateManualReportCommand> orders, CancellationToken cancellationToken)
    {
        var orderReport = await _mediator.Send(new CreateManualReportCommand.Command(orders, clientId, taxesId), cancellationToken);

        return Ok(orderReport);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOrderReport(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOrderReportCommand(id), cancellationToken);

        return NoContent();
    }
}
