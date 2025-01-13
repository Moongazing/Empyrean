using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IPayrollRepository : IAsyncRepository<PayrollEntity, Guid>, IRepository<PayrollEntity, Guid>
{
    Task<ICollection<PayrollEntity>> GetPayrollCurrentMonthAsync(Guid employeeId)
    Task<ICollection<PayrollEntity>> GetPayrollsByDateRangeAsync(Guid employeeId, DateTime startDate, DateTime endDate);
    Task<byte[]> ExportToExcelAsync(List<PayrollEntity> payrolls);
    Task<byte[]> ExportToWordAsync(List<PayrollEntity> payrolls);
}

