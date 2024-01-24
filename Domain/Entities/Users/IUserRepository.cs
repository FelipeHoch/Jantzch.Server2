using Jantzch.Server2.Application.Helpers;
using Jantzch.Server2.Application.Users;
using MongoDB.Bson;

namespace Jantzch.Server2.Domain.Entities.Users;

public interface IUserRepository
{
    Task<PagedList<User>> GetAsync(UsersResourceParameters parameters, CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken);

    Task<User?> GetByIdpIdAsync(ObjectId idpId, CancellationToken cancellationToken); 

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task AddAsync(User user, CancellationToken cancellationToken);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
