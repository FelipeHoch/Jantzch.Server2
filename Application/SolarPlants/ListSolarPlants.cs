using AutoMapper;
using Jantzch.Server2.Domain.Entities.SolarPlants;
using MediatR;

namespace Jantzch.Server2.Application.SolarPlants;

public class ListSolarPlants
{
    public record Query : IRequest<IEnumerable<SolarPlantResponse>>;

    public class Handler(
        ISolarPlantRepository solarPlantRepository,
        IMapper mapper
    ) : IRequestHandler<Query, IEnumerable<SolarPlantResponse>>
    {
        public async Task<IEnumerable<SolarPlantResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var solarPlants = await solarPlantRepository.GetAllAsync();

            return mapper.Map<IEnumerable<SolarPlantResponse>>(solarPlants);
        }
    }
}
