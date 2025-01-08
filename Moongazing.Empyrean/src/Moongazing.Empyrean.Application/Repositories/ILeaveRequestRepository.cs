using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface ILeaveRequestRepository : IAsyncRepository<LeaveRequestEntity, Guid>, IRepository<LeaveRequestEntity, Guid>
{
}

