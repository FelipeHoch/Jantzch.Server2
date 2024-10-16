using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.SolarPlants;
using Jantzch.Server2.Domain.Entities.SolarPlants.Constants;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.SolarPlants.SolarConsumers;

public class CreateSolarConsumer
{
    public class SolarConsumerForCreation
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Consumption { get; set; }
    }

    public record Command(string SolarPlantId, SolarConsumerForCreation SolarConsumerForCreation) : IRequest<SolarConsumerResponse>;

    public class Handler(
        ISolarPlantRepository solarPlantRepository,
        IJwtService jwtService,
        IMapper mapper
    ) : IRequestHandler<Command, SolarConsumerResponse>
    {
        public async Task<SolarConsumerResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new UserSimple
            {
                Id = jwtService.GetNameIdentifierFromToken(),
                Name = jwtService.GetNameFromToken()
            };

            var solarPlant = await solarPlantRepository.GetByIdAsync(request.SolarPlantId);

            if (solarPlant is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = SolarPlantErrorMessages.NOT_FOUND });
            }

            var solarConsumer = mapper.Map<SolarConsumer>(request.SolarConsumerForCreation);

            solarConsumer.CreatedBy = user.Id;

            if (!solarPlant.IsAvailableReceiveConsumer(solarConsumer))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { message = SolarPlantErrorMessages.EXCEEDS_CAPACITY });
            }

            await solarPlantRepository.CreateConsumerAsync(solarPlant.Id, solarConsumer);

            return mapper.Map<SolarConsumerResponse>(solarConsumer);
        }
    }
}
