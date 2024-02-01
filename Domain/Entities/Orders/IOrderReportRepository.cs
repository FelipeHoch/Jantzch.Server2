using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.OrderReports;

namespace Jantzch.Server2.Domain.Entities.Orders;

public interface IOrderReportRepository
{
    Task<PagedList<OrderReport>> GetAsync(OrderReportResourceParameters parameters, CancellationToken cancellationToken);

    Task<OrderReport?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task AddAsync(OrderReport orderReport, CancellationToken cancellationToken);

    Task<OrderReport?> LastReportInserted(CancellationToken cancellationToken);

    Task<bool> OrdersAlreadyHasReportLinked(List<string> orderIds);

    Task<long> CountReports(CancellationToken cancellationToken);

    Task DeleteReport(string id, CancellationToken cancellationToken);
}
