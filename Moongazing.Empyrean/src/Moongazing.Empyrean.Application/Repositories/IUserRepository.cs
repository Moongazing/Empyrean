using Moongazing.Kernel.Persistence.Repositories;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IUserRepository : IAsyncRepository<UserEntity, Guid>, IRepository<UserEntity, Guid>
{

}
