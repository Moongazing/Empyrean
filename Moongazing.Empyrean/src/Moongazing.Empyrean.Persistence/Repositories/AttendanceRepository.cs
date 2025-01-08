using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class AttendanceRepository : EfRepositoryBase<AttendanceEntity, Guid, BaseDbContext>, IAttendanceRepository
{
    public AttendanceRepository(BaseDbContext context) : base(context)
    {
    }
}
