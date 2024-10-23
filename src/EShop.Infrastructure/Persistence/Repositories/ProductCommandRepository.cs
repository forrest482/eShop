namespace EShop.Infrastructure.Persistence.Repositories;

public class ProductCommandRepository : IProductCommandRepository
{
    private readonly EShopDbContext _context;

    public ProductCommandRepository(EShopDbContext context)
    {
        _context = context;
        UnitOfWork = context;
    }

    public IUnitOfWork UnitOfWork { get; }

    public async Task<Product> GetByIdAsync(ProductId id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            throw new NotFoundException($"Product with ID {id} was not found.");

        return product;
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
    }

    public async Task DeleteAsync(ProductId id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
            _context.Products.Remove(product);
    }
}
