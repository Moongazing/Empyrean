using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Empyrean.Domain.Entities;

public class DepartmentEntity : Entity<Guid>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ICollection<BranchEntity> Branches { get; set; } = new HashSet<BranchEntity>();
    public DepartmentEntity()
    {

    }


}


