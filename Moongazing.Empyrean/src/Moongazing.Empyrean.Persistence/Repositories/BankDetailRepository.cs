using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class BankDetailRepository : EfRepositoryBase<BankDetailEntity, Guid, BaseDbContext>, IBankDetailRepository
{
    public BankDetailRepository(BaseDbContext context) : base(context)
    {
    }
}
