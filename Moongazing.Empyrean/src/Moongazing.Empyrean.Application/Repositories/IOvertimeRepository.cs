using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IOvertimeRepository : IAsyncRepository<OvertimeEntity, Guid>, IRepository<OvertimeEntity, Guid>
{
}

