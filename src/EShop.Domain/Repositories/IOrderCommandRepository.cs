namespace EShop.Domain.Repositories;

public interface IOrderCommandRepository : ICommandRepository<Order>
{
    Task<Order> GetByIdAsync(OrderId id);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(OrderId id);
}
