using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Kernel.Security.Models;

public class EmailAuthenticatorEntity : Entity<Guid>
{
    public Guid UserId { get; set; }
    public string? ActivationKey { get; set; }
    public bool IsVerified { get; set; }

    public virtual UserEntity User { get; set; } = null!;

    public EmailAuthenticatorEntity()
    {

    }

    public EmailAuthenticatorEntity(Guid userId, bool isVerified)
    {
        UserId = userId;
        IsVerified = isVerified;
    }

    public EmailAuthenticatorEntity(Guid id, Guid userId, bool isVerified)
        : base()
    {
        Id = id;
        UserId = userId;
        IsVerified = isVerified;
    }
}
