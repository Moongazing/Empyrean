using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class DisciplinaryActionRepository : EfRepositoryBase<DisciplinaryActionEntity, Guid, BaseDbContext>, IDisciplinaryActionRepository
{
    public DisciplinaryActionRepository(BaseDbContext context) : base(context)
    {
    }
}
