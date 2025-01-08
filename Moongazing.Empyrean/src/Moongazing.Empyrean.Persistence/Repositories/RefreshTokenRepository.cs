using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Persistence.Repositories;

public class RefreshTokenRepository : EfRepositoryBase<RefreshTokenEntity, int, BaseDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<List<RefreshTokenEntity>> GetOldRefreshTokensAsync(Guid userID, int refreshTokenTTL)
    {
        List<RefreshTokenEntity> tokens = await Query()
            .AsNoTracking()
            .Where(r =>
                r.UserId == userID
                && r.Revoked == null
                && r.Expires >= DateTime.UtcNow
                && r.CreatedDate.AddDays(refreshTokenTTL) <= DateTime.UtcNow
            )
            .ToListAsync();

        return tokens;
    }
}
