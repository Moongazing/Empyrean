using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IBenefitRepository : IAsyncRepository<BenefitEntity, Guid>, IRepository<BenefitEntity, Guid>
{
}

