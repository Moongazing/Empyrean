using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class TerminationRepository : EfRepositoryBase<TerminationEntity, Guid, BaseDbContext>, ITerminationRepository
{
    public TerminationRepository(BaseDbContext context) : base(context)
    {
    }
}
