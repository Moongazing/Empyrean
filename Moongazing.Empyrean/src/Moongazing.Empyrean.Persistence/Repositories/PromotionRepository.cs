using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class PromotionRepository : EfRepositoryBase<PromotionEntity, Guid, BaseDbContext>, IPromotionRepository
{
    public PromotionRepository(BaseDbContext context) : base(context)
    {
    }
}
