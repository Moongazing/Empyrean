using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class UserEmailAuthenticatorRepository : EfRepositoryBase<EmailAuthenticatorEntity, Guid, BaseDbContext>, IEmailAuthenticatorRepository
{
    public UserEmailAuthenticatorRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<ICollection<EmailAuthenticatorEntity>> DeleteAllNonVerifiedAsync(UserEntity user)
    {
        var nonVerifiedAuthenticators = await Query()
                                      .Where(uea => uea.UserId == user.Id && !uea.IsVerified)
                                      .ToListAsync();

        if (nonVerifiedAuthenticators.Count != 0)
        {
            await DeleteRangeAsync(nonVerifiedAuthenticators);
        }

        return nonVerifiedAuthenticators;
    }
}
