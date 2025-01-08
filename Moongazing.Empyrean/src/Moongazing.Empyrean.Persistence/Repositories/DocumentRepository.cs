using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Kernel.Persistence.Repositories.Base;

namespace Moongazing.Empyrean.Persistence.Repositories;

public class DocumentRepository : EfRepositoryBase<DocumentEntity, Guid, BaseDbContext>, IDocumentRepository
{
    public DocumentRepository(BaseDbContext context) : base(context)
    {
    }
}
