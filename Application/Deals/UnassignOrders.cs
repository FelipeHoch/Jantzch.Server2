using AutoMapper;
using Jantzch.Server2.Domain.Entities.Clients.Deals;
using Jantzch.Server2.Domain.Entities.Clients.Deals.Constants;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Orders.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.Deals;

public class UnassignOrders
{
    public record Command(string DealId, List<string> OrderIds) : IRequest<DealResponse>;

    public class Handler : IRequestHandler<Command, DealResponse>
    {
        private readonly IDealRepository _dealRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public Handler(IDealRepository dealRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _dealRepository = dealRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<DealResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var deal = await _dealRepository.GetByIdAsync(request.DealId, cancellationToken)
                ?? throw new RestException(HttpStatusCode.NotFound, new { message = DealErrorMessages.NOT_FOUND });

            var orders = await _orderRepository.GetByIdsAsync(request.OrderIds, cancellationToken);
            
            if (orders.Count != request.OrderIds.Count)
            {
                var foundIds = orders.Select(o => o.Id).ToList();
                var notFoundIds = request.OrderIds.Except(foundIds);
                throw new RestException(HttpStatusCode.NotFound, new { message = OrdersErrorMessages.NOT_FOUND, notFoundIds });
            }

            foreach (var order in orders)
            {
                if (order.DealId != request.DealId)
                {
                    throw new RestException(HttpStatusCode.BadRequest, 
                        new { message = $"Order {order.Id} is not associated with deal {request.DealId}" });
                }

                deal.RemoveOrder(order.Id);
                order.RemoveFromDeal();
                await _orderRepository.UpdateAsync(order, cancellationToken);
            }

            await _dealRepository.UpdateAsync(deal, cancellationToken);

            return _mapper.Map<DealResponse>(deal);
        }
    }
} 