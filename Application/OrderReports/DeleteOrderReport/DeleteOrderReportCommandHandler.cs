using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Orders.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;
using System.Net;

namespace Jantzch.Server2.Application.OrderReports.DeleteOrderReport;

public class DeleteOrderReportCommandHandler : IRequestHandler<DeleteOrderReportCommand>
{
    private readonly IOrderReportRepository _orderReportRepository;

    private readonly IOrderRepository _orderRepository;

    public DeleteOrderReportCommandHandler(IOrderReportRepository orderReportRepository, IOrderRepository orderRepository)
    {
        _orderReportRepository = orderReportRepository;

        _orderRepository = orderRepository;
    }

    public async Task Handle(DeleteOrderReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _orderReportRepository.GetByIdAsync(request.Id, cancellationToken);

        if (report is null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { message = OrdersErrorMessages.REPORT_NOT_FOUND });
        }

        var ordersIds = report.Orders.Select(x => x.Id).ToList();

        await _orderReportRepository.DeleteReport(report.Id, cancellationToken);

        await _orderRepository.UpdateToNoReportedAsync(ordersIds.ToArray(), cancellationToken);
    }
}
