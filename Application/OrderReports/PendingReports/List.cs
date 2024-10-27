using AutoMapper;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.ReportConfigurations.Constants;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Application.Abstractions.Jwt;

namespace Jantzch.Server2.Application.OrderReports.PendingReports;

public class List
{
    public class Query : IRequest<List<ClientWithPendingValue>>
    {
    }

    public class Handler(
        IOrderReportRepository orderReportRepository,
        IClientsRepository clientsRepository,
        IReportConfigurationRepository reportConfRepository,
        IOrderRepository orderRepository,
        ITaxesRepository taxesRepository,
        IJwtService jwtService,
        IMapper mapper
    ) : IRequestHandler<Query, List<ClientWithPendingValue>>
    {
        public async Task<List<ClientWithPendingValue>> Handle(Query request, CancellationToken cancellationToken)
        {
            var notExportedOrders = await orderRepository.GetOrderPendingToExportAsync(cancellationToken);     

            var clientIds = notExportedOrders.Select(order => new ObjectId(order.Client.Id)).Distinct().ToList();

            var clients = await clientsRepository.GetByIdsAsync(clientIds, cancellationToken);

            var reportConfig = await reportConfRepository.GetByKeyAsync("ORDER", cancellationToken);

            if (reportConfig is null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { message = ReportConfErrorMessages.NOT_FOUND });
            }

            List<ClientWithPendingValue> clientWithPendingValues = [];

            clients.ForEach(client =>
            {
                var pendingValue = new ClientWithPendingValue{
                    Client = new ClientSimple
                    {
                        Id = client.Id,
                        Name = client.Name,
                        Address = client.Localizations.Find(localization => localization.IsPrimary).Address,
                        Location = client.Localizations.Find(localization => localization.IsPrimary).Location,
                        PhoneNumber = client.PhoneNumber,
                        Route = client.Localizations.Find(localization => localization.IsPrimary).Route
                    },
                    PendingValue = 0
                };

                var orders = notExportedOrders.Where(order => order.Client.Id == client.Id).ToList();

                orders.ForEach(order =>
                {
                    var orderToExport = new OrderExport(order, reportConfig.MinValue);

                    pendingValue.PendingValue += orderToExport.MaterialCust;

                    pendingValue.PendingValue += orderToExport.ManPower;

                    pendingValue.OrdersToExport.Add(orderToExport);
                });


                clientWithPendingValues.Add(pendingValue);
            });

            return clientWithPendingValues;
        }
    }
}
