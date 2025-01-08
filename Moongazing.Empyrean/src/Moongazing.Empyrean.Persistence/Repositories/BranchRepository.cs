using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class BranchRepository : EfRepositoryBase<BranchEntity, Guid, BaseDbContext>, IBranchRepository
{
    public BranchRepository(BaseDbContext context) : base(context)
    {
    }
}
