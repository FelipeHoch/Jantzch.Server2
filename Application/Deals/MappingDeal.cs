using AutoMapper;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using static Jantzch.Server2.Application.Deals.CreateDeal;
using static Jantzch.Server2.Application.Deals.EditDeal;

namespace Jantzch.Server2.Application.Deals;

public class MappingDeal : Profile
{
    public MappingDeal()
    {
        CreateMap<Deal, DealResponse>();

        CreateMap<DealForCreation, Deal>();

        CreateMap<DealForEdit, Deal>();
    }
}
