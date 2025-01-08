using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IBranchRepository : IAsyncRepository<BranchEntity, Guid>, IRepository<BranchEntity, Guid>
{
}

