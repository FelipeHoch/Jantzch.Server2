﻿using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.Orders.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReportRepository _orderReportRepository;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, IOrderReportRepository orderReportRepository)
    {
        _orderRepository = orderRepository;
        _orderReportRepository = orderReportRepository;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = "Ordem não encontrada" });
        }

        var reportLinked = await _orderReportRepository.OrdersAlreadyHasReportLinked([request.Id]);

        if (reportLinked)
        {
            throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = "Ordem já possui relatório vinculado, exclua o relatório primeiro" });
        }

        await _orderRepository.DeleteAsync(order, cancellationToken);
    }
}
