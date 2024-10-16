using Jantzch.Server2.Domain.Entities.SolarPlants;
using Jantzch.Server2.Domain.Entities.SolarPlants.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.SolarPlants;

public class DeleteSolarPlant
{
    public record Command(string Id) : IRequest;

    public class Handler(
        ISolarPlantRepository solarPlantRepository
    ) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var solarPlant = await solarPlantRepository.GetByIdAsync(request.Id);

            if (solarPlant is null)
            {
                throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = SolarPlantErrorMessages.NOT_FOUND });
            }

            await solarPlantRepository.DeleteAsync(request.Id);
        }
    }
}
