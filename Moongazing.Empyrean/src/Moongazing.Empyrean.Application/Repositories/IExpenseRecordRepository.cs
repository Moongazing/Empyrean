using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IExpenseRecordRepository : IAsyncRepository<ExpenseRecordEntity, Guid>, IRepository<ExpenseRecordEntity, Guid>
{
}

