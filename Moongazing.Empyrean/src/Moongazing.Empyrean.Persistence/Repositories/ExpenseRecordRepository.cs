using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class ExpenseRecordRepository : EfRepositoryBase<ExpenseRecordEntity, Guid, BaseDbContext>, IExpenseRecordRepository
{
    public ExpenseRecordRepository(BaseDbContext context) : base(context)
    {
    }
}
