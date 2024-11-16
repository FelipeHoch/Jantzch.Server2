using MediatR;
using System.Dynamic;

namespace Jantzch.Server2.Application.Orders.GetOrder;

public record OrderQuery(string Id, string? Fields, string? DealId = null) : IRequest<ExpandoObject>;