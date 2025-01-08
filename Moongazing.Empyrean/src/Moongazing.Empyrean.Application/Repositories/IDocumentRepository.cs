using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Persistence.Repositories;

namespace Moongazing.Empyrean.Application.Repositories;

public interface IDocumentRepository : IAsyncRepository<DocumentEntity, Guid>, IRepository<DocumentEntity, Guid>
{
}

