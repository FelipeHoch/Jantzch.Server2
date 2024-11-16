using Jantzch.Server2.Application.Deals;
using Jantzch.Server2.Application.Deals.Analytics;
using Jantzch.Server2.Application.Deals.Analytics.DTOs;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using static Jantzch.Server2.Application.Deals.UploadImages;

namespace Jantzch.Server2.Api.Controllers.Deals;

[Route("api/[controller]")]
[Authorize(Roles = "admin,supervisor,normal")]
[ApiController]
public class DealsController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExpandoObject>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeals([FromQuery] DealsResourceParamenters parameters, CancellationToken cancellationToken)
    {
        var deals = await mediator.Send(new ListDeals.Query(parameters), cancellationToken);

        return Ok(deals);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DealResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDeal([FromBody] CreateDeal.DealForCreation deal, CancellationToken cancellationToken)
    {
        var createdDeal = await mediator.Send(new CreateDeal.Command(deal), cancellationToken);

        return CreatedAtAction(nameof(GetDeals), null, createdDeal);
    }

    [HttpPut("{dealId}")]
    [ProducesResponseType(typeof(DealResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditDeal(string dealId, [FromBody] EditDeal.DealForEdit deal, CancellationToken cancellationToken)
    {
        var editedDeal = await mediator.Send(new EditDeal.Command(dealId, deal), cancellationToken);

        return Ok(editedDeal);
    }

    [HttpPost("import")]
    [ProducesResponseType(typeof(IEnumerable<DealResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDeal([FromForm] ImportDeals.Command file, CancellationToken cancellationToken)
    {
        var deal = await mediator.Send(file, cancellationToken);

        return Ok(deal);
    }

    [HttpPost("{dealId}/next-status")]
    [ProducesResponseType(typeof(DealResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> NextStatus([FromQuery] StatusEnum status, [FromQuery] DateTime? date, string dealId, CancellationToken cancellationToken)
    {
        var deal = await mediator.Send(new ChangeStatus.Command(dealId, status, date), cancellationToken);

        return Ok(deal);
    }

    [HttpDelete("{dealId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDeal(string dealId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteDeal.Command(dealId), cancellationToken);

        return NoContent();
    }

    [HttpPost("{dealId}/upload-images")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UploadImages(string dealId, CancellationToken cancellationToken)
    {
        var formCollection = await Request.ReadFormAsync(cancellationToken);
        var files = formCollection.Files;

        var imageRequests = new List<UploadImageRequest>();

        for (int i = 0; i < files.Count; i++)
        {
            imageRequests.Add(new UploadImageRequest
            {
                Image = Request.Form.Files[$"file{i}"],
                Key = Enum.Parse<ImageKeyEnum>(Request.Form[$"key{i}"]),
                Description = Request.Form[$"description{i}"]
            });
        }

        await mediator.Send(new Command(dealId, imageRequests), cancellationToken);        

        return NoContent();
    }

    [HttpGet("{dealId}/images")]
    [ProducesResponseType(typeof(IEnumerable<Image>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListImages(string dealId, CancellationToken cancellationToken)
    {
        var images = await mediator.Send(new ListImages.Query(dealId), cancellationToken);

        return Ok(images);
    }

    [HttpGet("analytics/revenue-by-installation-type")]
    [ProducesResponseType(typeof(IEnumerable<DealAnalyticByInstallationType>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenueByInstallationType([FromQuery] AnalyticsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var deals = await mediator.Send(new RevenueByInstallationType.Query(parameters), cancellationToken);

        return Ok(deals);
    }

    [HttpGet("analytics/revenue-by-city")]
    [ProducesResponseType(typeof(IEnumerable<DealAnalyticByCity>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenueByCity([FromQuery] AnalyticsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var deals = await mediator.Send(new RevenueByCity.Query(parameters), cancellationToken);

        return Ok(deals);
    }

    [HttpGet("analytics/revenue-by-month")]
    [ProducesResponseType(typeof(IEnumerable<DealAnalyticByMonth>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenueByMonth([FromQuery] AnalyticsResourceParameters parameters, CancellationToken cancellationToken)
    {
        var deals = await mediator.Send(new RevenueByMonth.Query(parameters), cancellationToken);

        return Ok(deals);
    }

    [HttpGet("analytics/monthly-summary")]
    [ProducesResponseType(typeof(ComparativeIndicators), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMonthlySummary(CancellationToken cancellationToken)
    {
        var summary = await mediator.Send(new ActualMonthlySummary.Query(), cancellationToken);

        return Ok(summary);
    }

    [HttpPost("{dealId}/assign-orders")]
    [ProducesResponseType(typeof(DealResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignOrders(string dealId, [FromBody] List<string> orderIds, CancellationToken cancellationToken)
    {
        var deal = await mediator.Send(new AssignOrders.Command(dealId, orderIds), cancellationToken);
        return Ok(deal);
    }

    [HttpPost("{dealId}/unassign-orders")]
    [ProducesResponseType(typeof(DealResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UnassignOrders(string dealId, [FromBody] List<string> orderIds, CancellationToken cancellationToken)
    {
        var deal = await mediator.Send(new UnassignOrders.Command(dealId, orderIds), cancellationToken);
        return Ok(deal);
    }
}
