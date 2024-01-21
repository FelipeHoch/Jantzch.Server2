using AutoMapper;
using Jantzch.Server2.Domain.Entities.Clients;

namespace Jantzch.Server2.Application.Clients;

public class MappingClient : Profile
{
    public MappingClient()
    {
        CreateMap<Client, ClientResponse>();
    }
}
