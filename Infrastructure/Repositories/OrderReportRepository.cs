using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.OrderReports;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Domain.Entities.Orders;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class OrderReportRepository : IOrderReportRepository
{
    private readonly IMongoDatabase _database;

    private readonly IMongoCollection<OrderReport> _reports;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public OrderReportRepository(IMongoDatabase database, IPropertyCheckerService propertyCheckerService)
    {
        _database = database;
        _reports = _database.GetCollection<OrderReport>("orders_reports");
        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<OrderReport>> GetAsync(OrderReportResourceParameters parameters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var builder = Builders<OrderReport>.Filter;
        var filter = builder.Empty;
        var sort = Builders<OrderReport>.Sort.Descending(report => report.ReportNumber);

        if (!string.IsNullOrWhiteSpace(parameters.GeneratedBy))
        {
            var generatedByFilter = builder.Eq(report => report.GeneratedBy.ToLower(), parameters.GeneratedBy.ToLower());
            filter &= generatedByFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.Client))
        {
            var clientIdFilter = builder.Eq(report => report.Client.Name, parameters.Client);
            filter &= clientIdFilter;
        }

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {
            var searchQuery = parameters.SearchQuery.Trim().ToLower();

            var where = builder.Or(
                 builder.Regex(report => report.Client.Name, "/^" + searchQuery + "/i"),
                 builder.Regex(report => report.GeneratedBy, "/^" + searchQuery + "/i")
            );

            filter &= where;
        }

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            if (_propertyCheckerService.TypeHasProperties<OrderReport>(parameters.OrderBy))
            {
                sort = Builders<OrderReport>.Sort.Descending(parameters.OrderBy);
            }       
        }   

        var reports = _reports.Aggregate()
            .Match(filter)
            .Sort(sort);

        var totalReports = (int)await _reports.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return await PagedList<OrderReport>.CreateAsync(reports, parameters.PageNumber, parameters.PageSize, totalReports, cancellationToken);
    }

    public async Task<OrderReport?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {        
        return await _reports.Find(report => report.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(OrderReport orderReport, CancellationToken cancellationToken)
    {
        await _reports.InsertOneAsync(orderReport, cancellationToken: cancellationToken);
    }

    public async Task<OrderReport?> LastReportInserted(CancellationToken cancellationToken)
    {
        return await _reports.Find(report => true).SortByDescending(report => report.ReportNumber).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> OrdersAlreadyHasReportLinked(List<string> orderIds)
    {
        var ordersIdParsed = orderIds.Select(el => ObjectId.Parse(el)).ToList();

        var filter = Builders<OrderReport>.Filter.In("orders._id", ordersIdParsed);

        var alreadyHasReport = await _reports.Find(filter).FirstOrDefaultAsync();

        return alreadyHasReport != null;
    }

    public async Task<long> CountReports(CancellationToken cancellationToken)
    {
        return await _reports.CountDocumentsAsync(r => r.Id != null, cancellationToken: cancellationToken);
    }

    public async Task DeleteReport(string id, CancellationToken cancellationToken)
    {
        await _reports.DeleteOneAsync(report => report.Id == id, cancellationToken: cancellationToken);
    }
}
