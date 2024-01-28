using AutoMapper;
using Jantzch.Server2.Application.Users.Models;
using Jantzch.Server2.Domain.Entities.Users;

namespace Jantzch.Server2.Application.Users;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<User, UserResponse>();            

        CreateMap<UserFromIdpDto, User>()
            .ForMember(dest => dest.IdentityProviderId, opt => opt.MapFrom(src => src.Id));
    }
}
