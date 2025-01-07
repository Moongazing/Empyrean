namespace Moongazing.Kernel.Persistence.Repositories.Common;

public interface IEntityTimestampsMetadata
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
    DateTime? DeletedDate { get; set; }
}
