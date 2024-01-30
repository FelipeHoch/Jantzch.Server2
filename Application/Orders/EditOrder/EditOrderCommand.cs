using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Jantzch.Server2.Application.Orders.EditOrder;

public class EditOrderCommand
{
    public record Command(JsonPatchDocument<Order> Model, string Id) : IRequest<Order>;
}
