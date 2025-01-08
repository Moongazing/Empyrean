using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class BenefitRepository : EfRepositoryBase<BenefitEntity, Guid, BaseDbContext>, IBenefitRepository
{
    public BenefitRepository(BaseDbContext context) : base(context)
    {
    }
}
