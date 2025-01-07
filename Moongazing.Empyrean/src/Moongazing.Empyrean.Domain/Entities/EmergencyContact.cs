using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Empyrean.Domain.Entities;

public class EmergencyContact:Entity<Guid>
{
    public string Name { get; set; } = default!; 
    public string PhoneNumber { get; set; } = default!; 
    public string Relation { get; set; } = default!; 
    public Guid EmployeeId { get; set; }
    public virtual EmployeeEntity Employee { get; set; } = default!;

    public EmergencyContact()
    {

    }
}


