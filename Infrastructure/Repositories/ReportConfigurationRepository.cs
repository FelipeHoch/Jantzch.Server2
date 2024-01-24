using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Shared;
using Jantzch.Server2.Domain.Entities.ReportConfigurations;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class ReportConfigurationRepository : IReportConfigurationRepository
{
    private readonly JantzchContext _context;

    public ReportConfigurationRepository(JantzchContext context)
    {
        _context = context;
    }

    public async Task<PagedList<ReportConfiguration>> GetAsync(ResourceParameters parameters, CancellationToken cancellationToken)
    {
        var query = _context.ReportConfiguration.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {
            query = query.Where(x => x.ReportKey.Contains(parameters.SearchQuery));
        }

        return await PagedList<ReportConfiguration>.CreateAsync(query, parameters?.PageNumber ?? 1, parameters?.PageSize ?? 10, cancellationToken);
    }

    public async Task<ReportConfiguration?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await _context.ReportConfiguration
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ReportConfiguration?> GetByKeyAsync(string Key, CancellationToken cancellationToken)
    {
        return await _context.ReportConfiguration
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ReportKey == Key, cancellationToken);
    }

    public async Task AddAsync(ReportConfiguration reportConfiguration, CancellationToken cancellationToken)
    {
        await _context.ReportConfiguration.AddAsync(reportConfiguration, cancellationToken);
    }

    public async Task UpdateAsync(ReportConfiguration reportConfiguration)
    {
        _context.ReportConfiguration.Update(reportConfiguration);

        await Task.FromResult(Unit.Value);
    }

    public async Task DeleteAsync(ReportConfiguration reportConfiguration)
    {
        _context.ReportConfiguration.Remove(reportConfiguration);

        await Task.FromResult(Unit.Value);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
