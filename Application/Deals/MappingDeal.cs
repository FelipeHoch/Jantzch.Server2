using AutoMapper;
using Jantzch.Server2.Domain.Entities.Clients.Deals;

namespace Jantzch.Server2.Application.Deals;

public class MappingDeal : Profile
{
    public MappingDeal()
    {
        CreateMap<Deal, DealResponse>();
    }
}
