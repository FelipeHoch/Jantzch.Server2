using AutoMapper;
using Jantzch.Server2.Application.Clients.CreateClient;
using Jantzch.Server2.Application.Orders.CreateOrder;
using Jantzch.Server2.Domain.Entities.Orders;

namespace Jantzch.Server2.Application.Orders;

public class MappingOrder : Profile
{
    public MappingOrder()
    {
        CreateMap<Order, OrderResponse>();
        CreateMap<CreateOrderCommand, Order>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}
