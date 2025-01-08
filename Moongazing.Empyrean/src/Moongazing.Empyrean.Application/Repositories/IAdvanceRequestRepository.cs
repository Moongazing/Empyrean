using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;
using System;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IAdvanceRequestRepository : IAsyncRepository<AdvanceRequestEntity, Guid>, IRepository<AdvanceRequestEntity, Guid>
{
}

