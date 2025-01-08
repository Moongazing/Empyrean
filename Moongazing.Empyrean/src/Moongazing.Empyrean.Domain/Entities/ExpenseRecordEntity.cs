using Moongazing.Empyrean.Domain.Enums;
using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Empyrean.Domain.Entities;

public class ExpenseRecordEntity : Entity<Guid>
{
    public Guid EmployeeId { get; set; }
    public ExpenseType ExpenseType { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Description { get; set; } = default!;
    public DateTime ExpenseDate { get; set; }
    public bool IsReimbursed { get; set; }
    public virtual EmployeeEntity Employee { get; set; } = default!;

    public ExpenseRecordEntity()
    {

    }

}


