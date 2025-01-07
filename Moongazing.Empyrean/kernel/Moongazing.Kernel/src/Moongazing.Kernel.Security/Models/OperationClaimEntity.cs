using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Kernel.Security.Models;

public class OperationClaimEntity : Entity<Guid>
{
    public string Name { get; set; }
    public virtual ICollection<UserOperationClaimEntity> UserOperationClaims { get; set; } = null!;

    public OperationClaimEntity()
    {
        Name = string.Empty;
    }

    public OperationClaimEntity(string name)
    {
        Name = name;
    }

    public OperationClaimEntity(Guid id, string name)
        : base()
    {
        Id = id;
        Name = name;
    }
}