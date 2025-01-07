using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Kernel.Security.Models;

public class OtpAuthenticatorEntity : Entity<Guid>
{
    public Guid UserId { get; set; }
    public byte[] SecretKey { get; set; }
    public bool IsVerified { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public OtpAuthenticatorEntity()
    {
        SecretKey = [];
    }

    public OtpAuthenticatorEntity(Guid userId, byte[] secretKey, bool isVerified)
    {
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = isVerified;
    }

    public OtpAuthenticatorEntity(Guid id, Guid userId, byte[] secretKey, bool isVerified)
        : base()
    {
        Id = id;
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = isVerified;
    }
}