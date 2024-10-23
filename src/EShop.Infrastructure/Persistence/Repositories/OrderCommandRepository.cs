
namespace EShop.Infrastructure.Persistence.Repositories;

public class OrderCommandRepository : IOrderCommandRepository
{
    private readonly EShopDbContext _context;

    public OrderCommandRepository(EShopDbContext context)
    {
        _context = context;
        UnitOfWork = context;
    }

    public IUnitOfWork UnitOfWork { get; }

    public async Task<Order> GetByIdAsync(OrderId id)
    {
        return await _context.Orders
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        return order;
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
    }

    public async Task DeleteAsync(OrderId id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
            _context.Orders.Remove(order);
    }
}
