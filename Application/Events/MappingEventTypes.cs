using AutoMapper;
using Jantzch.Server2.Domain.Entities.Events;

namespace Jantzch.Server2.Application.Events;

public class MappingEventTypes : Profile
{
    public MappingEventTypes()
    {
        CreateMap<EventType, EventTypeResponse>();
    }
}
