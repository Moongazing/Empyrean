namespace Moongazing.Kernel.Persistence.Repositories.Common;

public abstract class Entity<TId> : IEntity<TId>, IEntityTimestampsMetadata
{
    public TId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public TId CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public TId? UpdatedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public TId? DeletedBy { get; set; }

    public Entity()
    {
        Id = default!;
        CreatedBy = default!;
     
    }

    public Entity(TId id, TId createdBy)
    {
        Id = id;
        CreatedBy = createdBy;
    }

}
