namespace Moongazing.Kernel.Persistence.Repositories.Common;

public interface IEntity<T>
{
    T Id { get; set; }
}
