using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class AwardRepository : EfRepositoryBase<AwardEntity, Guid, BaseDbContext>, IAwardRepository
{
    public AwardRepository(BaseDbContext context) : base(context)
    {
    }
}
