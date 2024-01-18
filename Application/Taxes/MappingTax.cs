using AutoMapper;
using Jantzch.Server2.Domain.Entities.Taxes;

namespace Jantzch.Server2.Application.Taxes;

public class MappingTax : Profile
{
    public MappingTax()
    {
        CreateMap<Tax, TaxResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MongoId));
    }
}
