using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Shared;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.ReportConfigurations;

public interface IReportConfigurationRepository
{
    Task<PagedList<ReportConfiguration>> GetAsync(ResourceParameters parameters, CancellationToken cancellationToken);

    Task<ReportConfiguration?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken);

    Task<ReportConfiguration?> GetByKeyAsync(string Key, CancellationToken cancellationToken);

    Task AddAsync(ReportConfiguration reportConfiguration, CancellationToken cancellationToken);

    Task UpdateAsync(ReportConfiguration reportConfiguration);

    Task DeleteAsync(ReportConfiguration reportConfiguration);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
