﻿using Moongazing.Empyrean.Domain.Enums;
using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Empyrean.Domain.Entities;

public class AdvanceRequestEntity : Entity<Guid>
{
    public Guid EmployeeId { get; set; }
    public decimal RequestedAmount { get; set; }
    public decimal ApprovedAmount { get; set; }
    public AdvanceRequestReason Reason { get; set; } = default!;
    public bool IsApproved { get; set; } = false;
    public DateTime RequestDate { get; set; }
    public DateTime? ApprovalDate { get; set; }

    public virtual EmployeeEntity Employee { get; set; } = default!;

    public AdvanceRequestEntity()
    {

    }

}
