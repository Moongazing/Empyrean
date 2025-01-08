using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class AdvanceRequestRepository : EfRepositoryBase<AdvanceRequestEntity, Guid, BaseDbContext>, IAdvanceRequestRepository
{
    public AdvanceRequestRepository(BaseDbContext context) : base(context)
    {
    }
}
