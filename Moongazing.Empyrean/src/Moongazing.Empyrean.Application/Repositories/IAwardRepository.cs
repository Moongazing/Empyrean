using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IAwardRepository : IAsyncRepository<AwardEntity, Guid>, IRepository<AwardEntity, Guid>
{
}

