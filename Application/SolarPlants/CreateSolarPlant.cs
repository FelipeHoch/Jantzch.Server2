using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.SolarPlants;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;

namespace Jantzch.Server2.Application.SolarPlants;

public class CreateSolarPlant
{
    public class SolarPlantForCreation
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }
    }

    public record Command(SolarPlantForCreation SolarPlantForCreation) : IRequest<SolarPlantResponse>;

    public class Handler(
        ISolarPlantRepository solarPlantRepository,
        IJwtService jwtService,
        IMapper mapper
    ) : IRequestHandler<Command, SolarPlantResponse>
    {
        public async Task<SolarPlantResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new UserSimple
            {
                Id = jwtService.GetNameIdentifierFromToken(),
                Name = jwtService.GetNameFromToken()
            };

            var solarPlant = mapper.Map<SolarPlant>(request.SolarPlantForCreation);

            solarPlant.CreatedBy = user;

            await solarPlantRepository.CreateAsync(solarPlant);

            return mapper.Map<SolarPlantResponse>(solarPlant);
        }
    }
}
