using AutoMapper;
using Jantzch.Server2.Domain.Entities.SolarPlants;

namespace Jantzch.Server2.Application.SolarPlants;

public class MappingSolarPlant : Profile
{
    public MappingSolarPlant()
    {
        CreateMap<SolarPlant, SolarPlantResponse>();

        CreateMap<SolarConsumer, SolarConsumerResponse>();
        
        CreateMap<CreateSolarPlant.SolarPlantForCreation, SolarPlant>();

        CreateMap<EditSolarPlant.SolarPlantForEdition, SolarPlant>();
    }
}
