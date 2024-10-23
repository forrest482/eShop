namespace EShop.Domain.SeedWork;

public interface ICommandRepository<T> where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
