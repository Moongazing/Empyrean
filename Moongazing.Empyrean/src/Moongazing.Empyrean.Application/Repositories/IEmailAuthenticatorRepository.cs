using Moongazing.Kernel.Persistence.Repositories;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IEmailAuthenticatorRepository : IAsyncRepository<EmailAuthenticatorEntity, Guid>, IRepository<EmailAuthenticatorEntity, Guid>
{
}
