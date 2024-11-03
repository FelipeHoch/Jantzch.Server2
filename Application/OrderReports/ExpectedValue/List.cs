using AutoMapper;
using Jantzch.Server2.Application.Orders;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Orders;
using MediatR;
using MongoDB.Bson;

namespace Jantzch.Server2.Application.OrderReports.ExpectedValue;

public class List
{
    public class Query : IRequest<List<ExpectedValueResponse>>
    {
    }

    public class Handler(
        IOrderRepository orderRepository,
        IClientsRepository clientsRepository,
        IMapper mapper
    ) : IRequestHandler<Query, List<ExpectedValueResponse>>
    {
        public async Task<List<ExpectedValueResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activeOrders = await orderRepository.GetActiveOrdersAsync(cancellationToken);

            var clientIds = activeOrders.Select(order => new ObjectId(order.Client.Id)).Distinct().ToList();

            var clients = await clientsRepository.GetByIdsAsync(clientIds, cancellationToken);

            List<ExpectedValueResponse> expectedValues = [];

            clients.ForEach(client =>
            {
                var expectedValue = new ExpectedValueResponse
                {
                    Client = new ClientSimple
                    {
                        Id = client.Id,
                        Name = client.Name,
                        Address = client.Localizations.Find(localization => localization.IsPrimary).Address,
                        Location = client.Localizations.Find(localization => localization.IsPrimary).Location,
                        PhoneNumber = client.PhoneNumber,
                        Route = client.Localizations.Find(localization => localization.IsPrimary).Route
                    },
                    ExpectedValue = 0
                };

                var orders = activeOrders.Where(order => order.Client.Id == client.Id).ToList();

                orders.ForEach(order =>
                {
                    expectedValue.ExpectedValue += order.ExpectedValue;

                    var orderResponse = mapper.Map<OrderResponse>(order);

                    expectedValue.Orders.Add(orderResponse);
                });

                expectedValues.Add(expectedValue);
            });

            return expectedValues;
        }
    }
}
