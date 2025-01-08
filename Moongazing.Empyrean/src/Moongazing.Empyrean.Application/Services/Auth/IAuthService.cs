using Moongazing.Kernel.Security.Jwt;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Services.Auth;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessToken(UserEntity user);
    public Task<RefreshTokenEntity> CreateRefreshToken(UserEntity user, string ipAddress);
    public Task<RefreshTokenEntity?> GetRefreshTokenByToken(string token);
    public Task<RefreshTokenEntity> AddRefreshToken(RefreshTokenEntity refreshToken);
    public Task DeleteOldRefreshTokens(Guid userId);
    public Task RevokeDescendantRefreshTokens(RefreshTokenEntity refreshToken, string ipAddress, string reason);
    public Task RevokeRefreshToken(RefreshTokenEntity token, string ipAddress, string? reason = null, string? replacedByToken = null);
    public Task<RefreshTokenEntity> RotateRefreshToken(UserEntity user, RefreshTokenEntity refreshToken, string ipAddress);

}
