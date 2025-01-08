using Microsoft.EntityFrameworkCore.Query;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Models;
using System.Linq.Expressions;

namespace Doing.Retail.Application.Services.User;

public interface IUserService
{
    Task<UserEntity?> GetAsync(
        Expression<Func<UserEntity, bool>> predicate,
        Func<IQueryable<UserEntity>, IIncludableQueryable<UserEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<IPagebale<UserEntity>?> GetListAsync(
        Expression<Func<UserEntity, bool>>? predicate = null,
        Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>? orderBy = null,
        Func<IQueryable<UserEntity>, IIncludableQueryable<UserEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<UserEntity> AddAsync(UserEntity user);
    Task<UserEntity> UpdateAsync(UserEntity existingUser);
    Task<UserEntity> DeleteAsync(UserEntity user, bool permanent = false);
}
