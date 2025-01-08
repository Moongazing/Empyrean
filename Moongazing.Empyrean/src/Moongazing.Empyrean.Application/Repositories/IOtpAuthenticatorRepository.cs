using Moongazing.Kernel.Persistence.Repositories;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IOtpAuthenticatorRepository : IAsyncRepository<OtpAuthenticatorEntity, Guid>, IRepository<OtpAuthenticatorEntity, Guid>
{
}
