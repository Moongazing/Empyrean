using Moongazing.Empyrean.Domain.Enums;
using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Empyrean.Domain.Entities;

public class AttendanceEntity : Entity<Guid>
{
    public Guid EmployeeId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; } = default!;

    public virtual EmployeeEntity Employee { get; set; } = default!;

    public AttendanceEntity()
    {

    }
}





