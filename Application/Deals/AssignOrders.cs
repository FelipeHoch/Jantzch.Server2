using AutoMapper;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Constants;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Orders.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.Deals;

public class AssignOrders
{
    public record Command(string DealId, List<string> OrderIds) : IRequest<DealResponse>;

    public class Handler(IDealRepository dealRepository, IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<Command, DealResponse>
    {
        public async Task<DealResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var deal = await dealRepository.GetByIdAsync(request.DealId, cancellationToken)
                ?? throw new RestException(HttpStatusCode.NotFound, new { message = DealErrorMessages.NOT_FOUND });

            var orders = await orderRepository.GetByIdsAsync(request.OrderIds, cancellationToken);
            
            if (orders.Count != request.OrderIds.Count)
            {
                var foundIds = orders.Select(o => o.Id).ToList();
                var notFoundIds = request.OrderIds.Except(foundIds);
                throw new RestException(HttpStatusCode.NotFound, new { message = OrdersErrorMessages.NOT_FOUND, notFoundIds });
            }

            foreach (var order in orders)
            {
                deal.AddOrder(order);
                order.AssignToDeal(deal.Id);
                await orderRepository.UpdateAsync(order, cancellationToken);
            }

            await dealRepository.UpdateAsync(deal, cancellationToken);

            return mapper.Map<DealResponse>(deal);
        }
    }
} 