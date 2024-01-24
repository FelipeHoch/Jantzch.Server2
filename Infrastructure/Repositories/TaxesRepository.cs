using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Application.Taxes;
using Jantzch.Server2.Domain.Entities.Taxes;
using System.Linq.Dynamic.Core;
using Jantzch.Server2.Infraestructure;
using MediatR;
using MongoDB.Bson;
using Microsoft.EntityFrameworkCore;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class TaxesRepository : ITaxesRepository
{
    private readonly JantzchContext _context;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public TaxesRepository(
        JantzchContext context,
        IPropertyCheckerService propertyCheckerService
    )
    {
        _context = context;

        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<Tax>> GetTaxesAsync(TaxesResourceParameters parameters, CancellationToken cancellationToken)
    {
        var query = _context.Taxes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Type))
        {
            query = query.Where(x => x.Type == parameters.Type);
        }

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {
            query = query.Where(x => x.Name.Contains(parameters.SearchQuery));
        }

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy) && _propertyCheckerService.TypeHasProperties<Tax>(parameters.OrderBy))
        {
            query = query.OrderBy(parameters.OrderBy + " dcending");
        }

        return await PagedList<Tax>.CreateAsync(query, parameters?.PageNumber ?? 1, parameters?.PageSize ?? 10, cancellationToken);
    }

    public async Task<Tax?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await _context.Taxes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Tax?> LastTaxInsertedAsync(CancellationToken cancellationToken)
    {
        return await _context.Taxes
            .AsNoTracking()
            .OrderByDescending(x => x.Code)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tax tax)
    {
        _context.Taxes.Update(tax);

        await Task.FromResult(Unit.Value);
    }

    public async Task AddAsync(Tax tax, CancellationToken cancellationToken)
    {
        await _context.Taxes.AddAsync(tax, cancellationToken);

        await Task.FromResult(Unit.Value);
    }

    public async Task DeleteAsync(Tax tax)
    {
        _context.Taxes.Remove(tax);

        await Task.FromResult(Unit.Value);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
