using Doing.Retail.Application.Services.Repositories;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class OperationClaimRepository : EfRepositoryBase<OperationClaimEntity, Guid, BaseDbContext>, IOperationClaimRepository
{
    public OperationClaimRepository(BaseDbContext context) : base(context)
    {
    }
}
