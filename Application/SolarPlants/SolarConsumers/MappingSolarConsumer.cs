using AutoMapper;
using Jantzch.Server2.Domain.Entities.SolarPlants;

namespace Jantzch.Server2.Application.SolarPlants.SolarConsumers;

public class MappingSolarConsumer : Profile
{
    public MappingSolarConsumer()
    {
        CreateMap<SolarConsumer, SolarConsumerResponse>();

        CreateMap<CreateSolarConsumer.SolarConsumerForCreation, SolarConsumer>();

        CreateMap<EditSolarConsumer.SolarConsumerForEdition, SolarConsumer>();
    }
}
