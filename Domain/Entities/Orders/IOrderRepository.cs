﻿using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.OrderReports.Models;
using Jantzch.Server2.Application.Orders;

namespace Jantzch.Server2.Domain.Entities.Orders;

public interface IOrderRepository
{
    Task<PagedList<Order>> GetAsync(OrderResourceParameters parameters, CancellationToken cancellationToken);

    Task<List<DetailedOrderForExport>> GetOrderPendingToExportAsync(CancellationToken cancellationToken);

    Task<List<Order>> GetActiveOrdersAsync(CancellationToken cancellationToken);

    Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<List<DetailedOrderForExport>> GetToExport(List<string> ordersId);

    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task UpdateAsync(Order order, CancellationToken cancellationToken);

    Task UpdateToReportedAsync(string[] ids, CancellationToken cancellationToken);

    Task UpdateToNoReportedAsync(string[] ids, CancellationToken cancellationToken);

    Task DeleteAsync(Order order, CancellationToken cancellationToken);

    Task<Order> LastOrderInserted(CancellationToken cancellationToken);

    Task<long> CountOrders(CancellationToken cancellationToken);

    Task<List<Order>> GetByIdsAsync(List<string> ids, CancellationToken cancellationToken);
}
