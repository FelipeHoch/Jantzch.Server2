using AutoMapper;
using Jantzch.Server2.Domain.Entities.Clients;
using Jantzch.Server2.Domain.Entities.Clients.Constants;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Domain.Entities.ReportConfigurations.Constants;
using Jantzch.Server2.Domain.Entities.Taxes;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Jantzch.Server2.Application.OrderReports.CreateManualReport;

public class CreateManualReportCommandHandler : IRequestHandler<CreateManualReportCommand.Command, OrderReportResponse>
{
    private readonly IOrderReportRepository _orderReportRepository;

    private readonly IOrderRepository _orderRepository;

    private readonly IClientsRepository _clientRepository;

    private readonly IReportConfigurationRepository _reportConfRepository;

    private readonly ITaxesRepository _taxesRepository;

    private readonly IMapper _mapper;

    public CreateManualReportCommandHandler(IOrderReportRepository orderReportRepository, IOrderRepository orderRepository, IClientsRepository clientRepository, IReportConfigurationRepository reportConfRepository, ITaxesRepository taxesRepository, IMapper mapper)
    {
        _orderReportRepository = orderReportRepository;

        _orderRepository = orderRepository;

        _clientRepository = clientRepository;

        _reportConfRepository = reportConfRepository;

        _taxesRepository = taxesRepository;

        _mapper = mapper;
    }

    public async Task<OrderReportResponse> Handle(CreateManualReportCommand.Command request, CancellationToken cancellationToken)
    {
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

        var ordersToExport = _mapper.Map<List<CreateManualReportCommand>,List<OrderExport>>(request.ManualReports);

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
