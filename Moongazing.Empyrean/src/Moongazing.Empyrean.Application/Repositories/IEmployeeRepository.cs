using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IEmployeeRepository : IAsyncRepository<EmployeeEntity, Guid>, IRepository<EmployeeEntity, Guid>
{
}

