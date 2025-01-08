using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class LeaveRequestRepository : EfRepositoryBase<LeaveRequestEntity, Guid, BaseDbContext>, ILeaveRequestRepository
{
    public LeaveRequestRepository(BaseDbContext context) : base(context)
    {
    }
}
