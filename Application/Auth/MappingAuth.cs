using AutoMapper;
using Jantzch.Server2.Domain.Entities.Users;

namespace Jantzch.Server2.Application.Auth;

public class MappingAuth : Profile
{
    public MappingAuth()
    {
        CreateMap<User, AuthResponse>();
    }
}
