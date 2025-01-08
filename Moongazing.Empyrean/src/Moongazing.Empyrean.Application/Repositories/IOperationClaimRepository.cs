using Moongazing.Kernel.Persistence.Repositories;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IOperationClaimRepository : IAsyncRepository<OperationClaimEntity, Guid>, IRepository<OperationClaimEntity, Guid>
{
}
