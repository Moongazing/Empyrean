using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Empyrean.Domain.Entities;

public class AwardEntity : Entity<Guid>
{
    public string AwardName { get; set; } = default!; 
    public string Description { get; set; } = default!; 
    public DateTime AwardDate { get; set; } 
    public Guid EmployeeId { get; set; }
    public virtual EmployeeEntity Employee { get; set; } = default!;
    public AwardEntity()
    {

    }
}
