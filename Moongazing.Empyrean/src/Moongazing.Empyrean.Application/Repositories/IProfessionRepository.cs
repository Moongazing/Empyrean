using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IProfessionRepository : IAsyncRepository<ProfessionEntity, Guid>, IRepository<ProfessionEntity, Guid>
{
}

