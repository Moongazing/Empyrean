using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IPromotionRepository : IAsyncRepository<PromotionEntity, Guid>, IRepository<PromotionEntity, Guid>
{
}

