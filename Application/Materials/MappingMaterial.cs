using AutoMapper;
using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Domain.Entities.Materials;
using Jantzch.Server2.Features.Materials;

namespace Jantzch.Server2.Application.Materials;

public class MappingMaterial : Profile
{
    public MappingMaterial()
    {
        CreateMap<Material, MaterialResponse>()
            .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.MongoGroupId))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MongoId));       
    }
}
