using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IPerformanceReviewRepository : IAsyncRepository<PerformanceReviewEntity, Guid>, IRepository<PerformanceReviewEntity, Guid>
{
}

