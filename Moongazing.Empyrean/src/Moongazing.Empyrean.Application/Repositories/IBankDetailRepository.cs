using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IBankDetailRepository : IAsyncRepository<BankDetailEntity, Guid>, IRepository<BankDetailEntity, Guid>
{
}

