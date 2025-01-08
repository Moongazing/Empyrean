using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class OvertimeRepository : EfRepositoryBase<OvertimeEntity, Guid, BaseDbContext>, IOvertimeRepository
{
    public OvertimeRepository(BaseDbContext context) : base(context)
    {
    }
}
