﻿namespace Moongazing.Kernel.Persistence.Repositories.Common;

public abstract class Entity<TId> : IEntity<TId>, IEntityTimestampsMetadata
{
    public TId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public Entity()
    {
        Id = default!;
    }

    public Entity(TId id)
    {
        Id = id;
    }

}
