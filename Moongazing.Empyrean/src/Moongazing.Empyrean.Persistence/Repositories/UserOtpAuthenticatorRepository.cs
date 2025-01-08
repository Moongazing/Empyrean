using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class UserOtpAuthenticatorRepository : EfRepositoryBase<OtpAuthenticatorEntity, Guid, BaseDbContext>, IUserOtpAuthenticatorRepository
{
    public UserOtpAuthenticatorRepository(BaseDbContext context) : base(context)
    {
    }
}
