﻿using AutoMapper;
using Jantzch.Server2.Application.Abstractions.Jwt;
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

namespace Jantzch.Server2.Application.OrderReports.CreateOrderReport;

public class CreateOrderReportCommandHandler : IRequestHandler<CreateOrderReportCommand.Command, OrderReportResponse>
{
    private readonly IOrderReportRepository _orderReportRepository;

    private readonly IOrderRepository _orderRepository;

    private readonly IClientsRepository _clientRepository;

    private readonly IReportConfigurationRepository _reportConfRepository;

    private readonly ITaxesRepository _taxesRepository;

    private readonly IJwtService _jwtService;

    private readonly IMapper _mapper;

    public CreateOrderReportCommandHandler(
        IOrderReportRepository orderReportRepository, 
        IOrderRepository orderRepository, 
        IClientsRepository clientRepository, 
        IReportConfigurationRepository reportConfRepository, 
        ITaxesRepository taxesRepository, 
        IJwtService jwtService,
        IMapper mapper)
    {
        _orderReportRepository = orderReportRepository;

        _orderRepository = orderRepository;

        _clientRepository = clientRepository;

        _reportConfRepository = reportConfRepository;

        _taxesRepository = taxesRepository;

        _jwtService = jwtService;

        _mapper = mapper;
    }

    public async Task<OrderReportResponse> Handle(CreateOrderReportCommand.Command request, CancellationToken cancellationToken)
    {       
        var detailedOrders = await _orderRepository.GetToExport(request.OrdersId);

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
            var taxesId = request.TaxesId.Select(ObjectId.Parse).ToList();

            taxes = await _taxesRepository.GetByIds(taxesId, cancellationToken);
        }

        var clientSimple = new ClientSimple
        {
            Id = client.Id,
            Name = client.Name,
            Address = client.Localizations.Find(localization => localization.IsPrimary).Address,
            Location = client.Localizations.Find(localization => localization.IsPrimary).Location,
            PhoneNumber = client.PhoneNumber,
            Route = client.Localizations.Find(localization => localization.IsPrimary).Route
        };

        var report = new OrderReport(clientSimple, reportNumber, _jwtService.GetNameFromToken(), ordersToExport, taxes);           
        
        await _orderReportRepository.AddAsync(report, cancellationToken);

        await _orderRepository.UpdateToReportedAsync([.. request.OrdersId], cancellationToken);

        return _mapper.Map<OrderReport, OrderReportResponse>(report);
    }
}
