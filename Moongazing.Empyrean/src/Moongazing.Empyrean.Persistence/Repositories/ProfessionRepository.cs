using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class ProfessionRepository : EfRepositoryBase<ProfessionEntity, Guid, BaseDbContext>, IProfessionRepository
{
    public ProfessionRepository(BaseDbContext context) : base(context)
    {
    }
}
