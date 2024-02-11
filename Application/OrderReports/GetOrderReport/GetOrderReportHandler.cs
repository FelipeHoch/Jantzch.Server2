using AutoMapper;
using Jantzch.Server2.Domain.Entities.Orders;
using Jantzch.Server2.Domain.Entities.Orders.Constants;
using Jantzch.Server2.Infraestructure.Errors;
using MediatR;

namespace Jantzch.Server2.Application.OrderReports.GetOrderReport;

public class GetOrderReportHandler : IRequestHandler<OrderReportQuery, OrderReportResponse>
{
    private readonly IOrderReportRepository _orderReportRepository;

    private readonly IMapper _mapper;

    public GetOrderReportHandler(IOrderReportRepository orderReportRepository, IMapper mapper)
    {
        _orderReportRepository = orderReportRepository;
        _mapper = mapper;
    }

    public async Task<OrderReportResponse> Handle(OrderReportQuery request, CancellationToken cancellationToken)
    {
        var report = await _orderReportRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (report is null)
        {
            throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = OrdersErrorMessages.REPORT_NOT_FOUND });
        }

        return _mapper.Map<OrderReportResponse>(report);
    }
}
