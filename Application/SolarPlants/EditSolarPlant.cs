using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.SolarPlants;
using Jantzch.Server2.Domain.Entities.SolarPlants.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.SolarPlants;

public class EditSolarPlant
{
    public class SolarPlantForEdition
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public bool IsActived { get; set; }
    }

    public record Command(string Id, SolarPlantForEdition SolarPlantForEdition) : IRequest<SolarPlantResponse>;

    public class Handler(
        ISolarPlantRepository solarPlantRepository,
        IJwtService jwtService,
        IMapper mapper
    ) : IRequestHandler<Command, SolarPlantResponse>
    {
        public async Task<SolarPlantResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var solarPlant = await solarPlantRepository.GetByIdAsync(request.Id);

            if (solarPlant is null)
            {
                throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = SolarPlantErrorMessages.NOT_FOUND });
            }      

            mapper.Map(request.SolarPlantForEdition, solarPlant);

            if (!solarPlant.IsCapacityValid(solarPlant.Capacity))
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = SolarPlantErrorMessages.EXCEEDS_CAPACITY });
            }

            solarPlant.LastUpdateAt = DateTime.UtcNow;

            await solarPlantRepository.UpdateAsync(solarPlant);

            return mapper.Map<SolarPlantResponse>(solarPlant);
        }
    }
}
