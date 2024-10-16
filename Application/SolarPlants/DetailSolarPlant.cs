using AutoMapper;
using Jantzch.Server2.Domain.Entities.SolarPlants;
using Jantzch.Server2.Domain.Entities.SolarPlants.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.SolarPlants;

public class DetailSolarPlant
{
    public record Query(string Id) : IRequest<SolarPlantResponse>;

    public class Handler(
        ISolarPlantRepository solarPlantRepository,
        IMapper mapper
    ) : IRequestHandler<Query, SolarPlantResponse>
    {
        public async Task<SolarPlantResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var solar = await solarPlantRepository.GetByIdAsync(request.Id);

            if (solar is null)
            {
                throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = SolarPlantErrorMessages.NOT_FOUND });
            }

            return mapper.Map<SolarPlantResponse>(solar);
        }
    }
}
