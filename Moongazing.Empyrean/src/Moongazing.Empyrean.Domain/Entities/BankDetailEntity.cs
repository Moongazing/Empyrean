using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Empyrean.Domain.Entities;

public class BankDetailEntity: Entity<Guid>
{
    public string BankName { get; set; } = default!; 
    public string AccountNumber { get; set; } = default!; 
    public string IBAN { get; set; } = default!; 
    public Guid EmployeeId { get; set; }
    public virtual EmployeeEntity Employee { get; set; } = default!;

    public BankDetailEntity()
    {

    }
}
