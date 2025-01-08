using Moongazing.Kernel.Persistence.Repositories;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Application.Services.Repositories;

public interface IUserOperationClaimRepository : IAsyncRepository<UserOperationClaimEntity, Guid>, IRepository<UserOperationClaimEntity, Guid>
{
    Task<IList<OperationClaimEntity>> GetOperationClaimsByUserIdAsync(Guid userId);
}