using Doing.Retail.Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Security.Jwt;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly ITokenHelper tokenHelper;
    private readonly TokenOptions tokenOptions;
    private readonly IUserOperationClaimRepository userOperationClaimRepository;

    public AuthService(IUserOperationClaimRepository userOperationClaimRepository,
                       IRefreshTokenRepository refreshTokenRepository,
                       ITokenHelper tokenHelper,
                       IConfiguration configuration)
    {
        this.userOperationClaimRepository = userOperationClaimRepository;
        this.refreshTokenRepository = refreshTokenRepository;
        this.tokenHelper = tokenHelper;

        const string tokenOptionsConfigurationSection = "TokenOptions";
        tokenOptions = configuration.GetSection(tokenOptionsConfigurationSection).Get<TokenOptions>()
            ?? throw new NullReferenceException($"\"{tokenOptionsConfigurationSection}\" section cannot found in configuration");
    }
    public async Task<AccessToken> CreateAccessToken(UserEntity user)
    {
        IList<OperationClaimEntity> operationClaims = await userOperationClaimRepository.GetOperationClaimsByUserIdAsync(user.Id);
        AccessToken accessToken = tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }
    public async Task<RefreshTokenEntity> AddRefreshToken(RefreshTokenEntity refreshToken)
    {
        RefreshTokenEntity addedRefreshToken = await refreshTokenRepository.AddAsync(refreshToken);
        return addedRefreshToken;
    }
    public async Task DeleteOldRefreshTokens(Guid userId)
    {
        List<RefreshTokenEntity> refreshTokens = await refreshTokenRepository
            .Query()
            .AsNoTracking()
            .Where(
                r =>
                    r.UserId == userId
                    && r.Revoked == null
                    && r.Expires >= DateTime.UtcNow
                    && r.CreatedDate.AddDays(tokenOptions.RefreshTokenTTL) <= DateTime.UtcNow
            )
            .ToListAsync();

        await refreshTokenRepository.DeleteRangeAsync(refreshTokens);
    }

    public async Task<RefreshTokenEntity?> GetRefreshTokenByToken(string token)
    {
        RefreshTokenEntity? refreshToken = await refreshTokenRepository.GetAsync(r => r.Token.ToLower() == token.ToLower());
        return refreshToken;
    }

    public async Task RevokeRefreshToken(RefreshTokenEntity existingRefreshToken, string ipAddress, string? reason = null, string? replacedByToken = null)
    {

        var updatedRefreshToken = new RefreshTokenEntity
        {
            Id = existingRefreshToken.Id,
            Token = existingRefreshToken.Token,
            Expires = existingRefreshToken.Expires,
            CreatedDate = existingRefreshToken.CreatedDate,
            CreatedByIp = existingRefreshToken.CreatedByIp,
            Revoked = DateTime.Now.Date,
            RevokedByIp = ipAddress,
            ReasonRevoked = reason,
            ReplacedByToken = replacedByToken,
            UserId = existingRefreshToken.UserId,
        };

        await refreshTokenRepository.UpdateAsync(existingRefreshToken);
    }


    public async Task<RefreshTokenEntity> RotateRefreshToken(UserEntity user, RefreshTokenEntity refreshToken, string ipAddress)
    {
        RefreshTokenEntity newRefreshToken = tokenHelper.CreateRefreshToken(user, ipAddress);
        await RevokeRefreshToken(refreshToken, ipAddress, reason: "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    public async Task RevokeDescendantRefreshTokens(RefreshTokenEntity refreshToken, string ipAddress, string reason)
    {
        RefreshTokenEntity? childToken = await refreshTokenRepository.GetAsync(predicate: r => r.Token == refreshToken.ReplacedByToken);

        if (childToken != null)
        {


            if (childToken.Revoked == null && childToken.Expires > DateTime.UtcNow)
            {
                await RevokeRefreshToken(childToken, ipAddress, reason);
            }
            else
            {
                await RevokeDescendantRefreshTokens(refreshToken: childToken, ipAddress, reason);
            }
        }
    }
    public Task<RefreshTokenEntity> CreateRefreshToken(UserEntity user, string ipAddress)
    {
        RefreshTokenEntity refreshToken = tokenHelper.CreateRefreshToken(user, ipAddress);
        return Task.FromResult(refreshToken);
    }
}
