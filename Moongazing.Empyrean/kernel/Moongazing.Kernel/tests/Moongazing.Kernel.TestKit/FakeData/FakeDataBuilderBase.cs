using Moongazing.Kernel.Persistence.Repositories.Common;

namespace Moongazing.Kernel.TestKit.FakeData;

public abstract class FakeDataBuilderBase<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>, new()
{
    public List<TEntity> Data => CreateFakeData();
    public abstract List<TEntity> CreateFakeData();
}

