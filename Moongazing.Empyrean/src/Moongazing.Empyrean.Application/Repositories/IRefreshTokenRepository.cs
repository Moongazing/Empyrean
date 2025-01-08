using Moongazing.Kernel.Persistence.Repositories;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IRefreshTokenRepository : IAsyncRepository<RefreshTokenEntity, int>, IRepository<RefreshTokenEntity, int>
{
    Task<List<RefreshTokenEntity>> GetOldRefreshTokensAsync(Guid userID, int refreshTokenTTL);

}
