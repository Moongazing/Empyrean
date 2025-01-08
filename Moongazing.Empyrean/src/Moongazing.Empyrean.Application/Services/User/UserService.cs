using Doing.Retail.Application.Features.Users.Rules;
using Microsoft.EntityFrameworkCore.Query;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Models;
using System.Linq.Expressions;

namespace Doing.Retail.Application.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly UserBusinessRules userBusinessRules;

    public UserService(IUserRepository userRepository,
                       UserBusinessRules userBusinessRules)
    {
        this.userRepository = userRepository;
        this.userBusinessRules = userBusinessRules;
    }

    public async Task<UserEntity?> GetAsync(Expression<Func<UserEntity, bool>> predicate,
                                            Func<IQueryable<UserEntity>, IIncludableQueryable<UserEntity, object>>? include = null,
                                            bool withDeleted = false,
                                            bool enableTracking = true,
                                            CancellationToken cancellationToken = default)
    {
        UserEntity? user = await userRepository.GetAsync(predicate,
                                                         include,
                                                         withDeleted,
                                                         enableTracking,
                                                         cancellationToken);
        return user;
    }

    public async Task<IPagebale<UserEntity>?> GetListAsync(Expression<Func<UserEntity, bool>>? predicate = null,
                                                          Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>? orderBy = null,
                                                          Func<IQueryable<UserEntity>, IIncludableQueryable<UserEntity, object>>? include = null,
                                                          int index = 0,
                                                          int size = 10,
                                                          bool withDeleted = false,
                                                          bool enableTracking = true,
                                                          CancellationToken cancellationToken = default)
    {
        IPagebale<UserEntity> userList = await userRepository.GetListAsync(predicate,
                                                                          orderBy,
                                                                          include,
                                                                          index,
                                                                          size,
                                                                          withDeleted,
                                                                          enableTracking,
                                                                          cancellationToken);
        return userList;
    }

    public async Task<UserEntity> AddAsync(UserEntity user)
    {
        await userBusinessRules.UserEmailShouldNotExistsWhenInsert(user.Email);

        UserEntity addedUser = await userRepository.AddAsync(user);

        return addedUser;
    }

    public async Task<UserEntity> UpdateAsync(UserEntity existingEntity)
    {

        var user = await userRepository.GetAsync(predicate: x => x.Id == existingEntity.Id);
        await userBusinessRules.UserIdShouldBeExistsWhenSelected(existingEntity.Id);

        await userRepository.UpdateAsync(user!);

        return user!;
    }

    public async Task<UserEntity> DeleteAsync(UserEntity user, bool permanent = true)
    {
        UserEntity deletedUser = await userRepository.DeleteAsync(user);

        return deletedUser;
    }
}
