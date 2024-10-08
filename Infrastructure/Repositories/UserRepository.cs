﻿using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Services.PropertyChecker;
using Jantzch.Server2.Application.Users;
using Jantzch.Server2.Domain.Entities.Users;
using Jantzch.Server2.Domain.Entities.Users.Enums;
using Jantzch.Server2.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Jantzch.Server2.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly JantzchContext _context;

    private readonly IPropertyCheckerService _propertyCheckerService;

    public UserRepository(JantzchContext context, IPropertyCheckerService propertyCheckerService)
    {
        _context = context;

        _propertyCheckerService = propertyCheckerService;
    }

    public async Task<PagedList<User>> GetAsync(UsersResourceParameters parameters, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Role))
        {
            query = query.Where(u => u.Role == parameters.Role);
        }

        if (!string.IsNullOrEmpty(parameters.Types))
        {
            var types = parameters.Types.Split(",").Select(t =>
            {
                var success = Enum.TryParse<UserTypeEnum>(t.Trim(), out var userType);
                return success ? userType : (UserTypeEnum?)null;
            })
            .Where(t => t != null)
            .Cast<UserTypeEnum>()
            .ToList();

            foreach (var type in types)
            {               
                query = query.Where(u => u.Types.Contains(type));
            }
        }

        if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
        {         
            query = query.Where(u => u.Email.Contains(parameters.SearchQuery) || u.Name.Contains(parameters.SearchQuery));
        }

        if (!string.IsNullOrWhiteSpace(parameters.OrderBy) && _propertyCheckerService.TypeHasProperties<User>(parameters.OrderBy))
        {
            query = query.OrderBy(parameters.OrderBy + " descending");
        }

        return await PagedList<User>.CreateAsync(query, parameters?.PageNumber ?? 1, parameters?.PageSize ?? 10, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<List<User>> GetByIdsAsync(List<string> ids, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id.ToString()))
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdpIdAsync(ObjectId idpId, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdentityProviderId.Equals(idpId), cancellationToken);
    }

    public async Task<List<User>> GetByIdpIdsAsync(List<string> idpIds, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(x => idpIds.Contains(x.IdentityProviderId.ToString()))
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);

        await Task.FromResult(Unit.Value);
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);

        await Task.FromResult(Unit.Value);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
