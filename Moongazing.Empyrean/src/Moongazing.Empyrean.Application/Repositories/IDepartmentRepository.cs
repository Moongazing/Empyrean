using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IDepartmentRepository : IAsyncRepository<DepartmentEntity, Guid>, IRepository<DepartmentEntity, Guid>
{
}

