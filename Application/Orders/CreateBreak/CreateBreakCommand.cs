using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;

namespace Jantzch.Server2.Application.Orders.CreateBreak;

public class CreateBreakCommand
{
    public string Descriptive { get; set; }
    
    public record Command(string Id, string Descriptive) : IRequest<Order>;
}
