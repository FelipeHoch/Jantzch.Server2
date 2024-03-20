using AutoMapper;

namespace Jantzch.Server2;

public class MappingPotentialOrder : Profile
{
    public MappingPotentialOrder()
    {
        CreateMap<PotentialOrder, PotentialOrderResponse>();
        CreateMap<CreatePotentialOrderCommand, PotentialOrder>();
    }
}
