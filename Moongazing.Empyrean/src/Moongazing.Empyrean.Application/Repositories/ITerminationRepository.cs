using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface ITerminationRepository : IAsyncRepository<TerminationEntity, Guid>, IRepository<TerminationEntity, Guid>
{
}

