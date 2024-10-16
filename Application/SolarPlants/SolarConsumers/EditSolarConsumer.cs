using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
using Jantzch.Server2.Domain.Entities.SolarPlants;
using Jantzch.Server2.Domain.Entities.SolarPlants.Constants;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.SolarPlants.SolarConsumers;

public class EditSolarConsumer
{
    public class SolarConsumerForEdition
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Consumption { get; set; }

        public bool IsActived { get; set; }
    }

    public record Command(string SolarPlantId, string SolarConsumerId, SolarConsumerForEdition SolarConsumerForEdition) : IRequest<SolarConsumerResponse>;

    public class Handler(
        ISolarPlantRepository solarPlantRepository,
        IMapper mapper
    ) : IRequestHandler<Command, SolarConsumerResponse>
    {
        public async Task<SolarConsumerResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var solarPlant = await solarPlantRepository.GetByIdAsync(request.SolarPlantId);

            if (solarPlant is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = SolarPlantErrorMessages.NOT_FOUND });
            }

            var solarConsumer = solarPlant.SolarConsumers.FirstOrDefault(x => x.Id == request.SolarConsumerId);

            if (solarConsumer is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = SolarPlantErrorMessages.INVALID_SOLAR_CONSUMER });
            }

            mapper.Map(request.SolarConsumerForEdition, solarConsumer);

            if (!solarPlant.IsAvailableReceiveConsumer(solarConsumer))
            {
                throw new RestException(HttpStatusCode.BadRequest, new { message = SolarPlantErrorMessages.EXCEEDS_CAPACITY });
            }

            solarPlant.LastUpdateAt = DateTime.UtcNow;

            await solarPlantRepository.UpdateConsumerAsync(request.SolarPlantId, solarConsumer);

            return mapper.Map<SolarConsumerResponse>(solarConsumer);
        }
    }
}
