using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IAdvanceRequestRepository : IAsyncRepository<AdvanceRequestEntity, Guid>, IRepository<AdvanceRequestEntity, Guid>
{
}

