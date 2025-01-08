using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class ShiftScheduleRepository : EfRepositoryBase<ShiftScheduleEntity, Guid, BaseDbContext>, IShiftScheduleRepository
{
    public ShiftScheduleRepository(BaseDbContext context) : base(context)
    {
    }
}
