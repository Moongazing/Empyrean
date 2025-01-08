using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IAttendanceRepository : IAsyncRepository<AttendanceEntity, Guid>, IRepository<AttendanceEntity, Guid>
{
}

