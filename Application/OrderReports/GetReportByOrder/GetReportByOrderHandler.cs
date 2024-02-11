using AutoMapper;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Orders.Constants;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Domain.Entities.ReportConfigurations.Constants;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.OrderReports.GetExportByOrder;

public class GetReportByOrderHandler : IRequestHandler<ReportByOrderQuery, OrderReportResponse>
{
    private readonly IOrderReportRepository _orderReportRepository;

    private readonly IOrderRepository _orderRepository;

    private readonly IClientsRepository _clientRepository;

    private readonly IReportConfigurationRepository _reportConfRepository;

    private readonly ITaxesRepository _taxesRepository;

    private readonly IMapper _mapper;

    public GetReportByOrderHandler(
        IOrderReportRepository orderReportRepository, 
        IOrderRepository orderRepository, 
        IClientsRepository clientRepository, 
        IReportConfigurationRepository reportConfRepository, 
        ITaxesRepository taxesRepository, 
        IMapper mapper)
    {
        _orderReportRepository = orderReportRepository;

        _orderRepository = orderRepository;

        _clientRepository = clientRepository;

        _reportConfRepository = reportConfRepository;

        _taxesRepository = taxesRepository;

        _mapper = mapper;
    }

    public async Task<OrderReportResponse> Handle(ReportByOrderQuery request, CancellationToken cancellationToken)
    {
        var hasAnyOrderAlreadyLinked = await _orderReportRepository.OrdersAlreadyHasReportLinked([request.OrderId]);

        if (hasAnyOrderAlreadyLinked)
        {
            throw new RestException(HttpStatusCode.BadRequest, new { message = OrdersErrorMessages.ORDER_ALREADY_REPORTED });
        }

        var detailedOrders = await _orderRepository.GetToExport([request.OrderId]);

        var client = await _clientRepository.GetByIdAsync(new ObjectId(request.ClientId), cancellationToken);

        if (client is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = ClientErrorMessages.NOT_FOUND });
        }

        var reportConfig = await _reportConfRepository.GetByKeyAsync("ORDER", cancellationToken);

        if (reportConfig is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = ReportConfErrorMessages.NOT_FOUND });
        }

        var lastReport = await _orderReportRepository.LastReportInserted(cancellationToken);

        var reportNumber = lastReport?.ReportNumber + 1 ?? 1;

        var ordersToExport = new List<OrderExport>();

        detailedOrders.ForEach(detailedOrder =>
        {
            var orderToExport = new OrderExport(detailedOrder, reportConfig.MinValue);

            ordersToExport.Add(orderToExport);
        });
        
        var taxes = new List<Tax>();

        if (request.TaxesId is not null)
        {
            var taxesId = request.TaxesId.Split(",").Select(ObjectId.Parse).ToList();

            taxes = await _taxesRepository.GetByIds(taxesId, cancellationToken);
        }

        var report = new OrderReport(client, reportNumber, "Mock", ordersToExport, taxes);

        return _mapper.Map<OrderReport, OrderReportResponse>(report);
    }
}
