using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IShiftScheduleRepository : IAsyncRepository<ShiftScheduleEntity, Guid>, IRepository<ShiftScheduleEntity, Guid>
{
}

