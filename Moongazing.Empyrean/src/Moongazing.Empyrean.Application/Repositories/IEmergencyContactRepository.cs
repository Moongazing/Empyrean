using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IEmergencyContactRepository : IAsyncRepository<EmergencyContactEntity, Guid>, IRepository<EmergencyContactEntity, Guid>
{
}

