using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class EmergencyContactRepository : EfRepositoryBase<EmergencyContactEntity, Guid, BaseDbContext>, IEmergencyContactRepository
{
    public EmergencyContactRepository(BaseDbContext context) : base(context)
    {
    }
}
