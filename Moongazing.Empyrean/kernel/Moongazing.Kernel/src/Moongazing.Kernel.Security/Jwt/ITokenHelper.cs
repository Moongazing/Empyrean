using Moongazing.Kernel.Security.Models;

namespace Moongazing.Kernel.Security.Jwt;

public interface ITokenHelper
{
    AccessToken CreateToken(UserEntity user, IList<OperationClaimEntity> operationClaims);
    RefreshTokenEntity CreateRefreshToken(UserEntity user, string ipAddress);
}
