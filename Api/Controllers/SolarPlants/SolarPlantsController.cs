using Jantzch.Server2.Application.SolarPlants;
using Jantzch.Server2.Application.SolarPlants.SolarConsumers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.SolarPlants;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "admin,supervisor")]
public class SolarPlantsController(
    IMediator mediator
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SolarPlantResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListSolarPlants(CancellationToken cancellationToken)
    {
        var solarPlants = await mediator.Send(new ListSolarPlants.Query(), cancellationToken);

        return Ok(solarPlants);
    }

    [HttpGet("{solarPlantId}")]
    [ProducesResponseType(typeof(SolarPlantResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSolarPlant(string solarPlantId, CancellationToken cancellationToken)
    {
        var solarPlant = await mediator.Send(new DetailSolarPlant.Query(solarPlantId), cancellationToken);

        return Ok(solarPlant);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SolarPlantResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSolarPlant([FromBody] CreateSolarPlant.SolarPlantForCreation solarPlant, CancellationToken cancellationToken)
    {
        var createdSolarPlant = await mediator.Send(new CreateSolarPlant.Command(solarPlant), cancellationToken);

        return CreatedAtAction(nameof(GetSolarPlant), new { solarPlantId = createdSolarPlant.Id }, createdSolarPlant);
    }

    [HttpPut("{solarPlantId}")]
    [ProducesResponseType(typeof(SolarPlantResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditSolarPlant(string solarPlantId, [FromBody] EditSolarPlant.SolarPlantForEdition solarPlant, CancellationToken cancellationToken)
    {
        var editedSolarPlant = await mediator.Send(new EditSolarPlant.Command(solarPlantId, solarPlant), cancellationToken);

        return Ok(editedSolarPlant);
    }

    [HttpDelete("{solarPlantId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSolarPlant(string solarPlantId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteSolarPlant.Command(solarPlantId), cancellationToken);

        return NoContent();
    }

    [HttpPost("{solarPlantId}/consumers")]
    [ProducesResponseType(typeof(SolarConsumerResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSolarConsumer(string solarPlantId, [FromBody] CreateSolarConsumer.SolarConsumerForCreation solarConsumer, CancellationToken cancellationToken)
    {
        var createdSolarConsumer = await mediator.Send(new CreateSolarConsumer.Command(solarPlantId, solarConsumer), cancellationToken);

        return CreatedAtAction(nameof(GetSolarPlant), new { solarPlantId }, createdSolarConsumer);
    }

    [HttpPut("{solarPlantId}/consumers/{solarConsumerId}")]
    [ProducesResponseType(typeof(SolarConsumerResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditSolarConsumer(string solarPlantId, string solarConsumerId, [FromBody] EditSolarConsumer.SolarConsumerForEdition solarConsumer, CancellationToken cancellationToken)
    {
        var editedSolarConsumer = await mediator.Send(new EditSolarConsumer.Command(solarPlantId, solarConsumerId, solarConsumer), cancellationToken);

        return Ok(editedSolarConsumer);
    }

    [HttpDelete("{solarPlantId}/consumers/{solarConsumerId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSolarConsumer(string solarPlantId, string solarConsumerId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteSolarConsumer.Command(solarPlantId, solarConsumerId), cancellationToken);

        return NoContent();
    }
}
