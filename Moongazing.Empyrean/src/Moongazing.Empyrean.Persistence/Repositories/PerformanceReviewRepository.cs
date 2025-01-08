using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class PerformanceReviewRepository : EfRepositoryBase<PerformanceReviewEntity, Guid, BaseDbContext>, IPerformanceReviewRepository
{
    public PerformanceReviewRepository(BaseDbContext context) : base(context)
    {
    }
}
