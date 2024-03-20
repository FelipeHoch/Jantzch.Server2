using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;

namespace Jantzch.Server2;

public record ConvertPotentialOrderCommand(string Id) : IRequest<Order>;
