using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class DepartmentRepository : EfRepositoryBase<DepartmentEntity, Guid, BaseDbContext>, IDepartmentRepository
{
    public DepartmentRepository(BaseDbContext context) : base(context)
    {
    }
}
