using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IDisciplinaryActionRepository : IAsyncRepository<DisciplinaryActionEntity, Guid>, IRepository<DisciplinaryActionEntity, Guid>
{
}

