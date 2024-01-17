using AutoMapper;
using Jantzch.Server2.Domain.Entities.GroupsMaterial;

namespace Jantzch.Server2.Application.GroupsMaterial;

public class MappingGroupMaterial : Profile
{
    public MappingGroupMaterial()
    {
        CreateMap<GroupMaterial, GroupMaterialResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MongoId));
    }
}
