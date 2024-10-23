using EShop.Application.Products.Abstractions;

namespace EShop.Infrastructure.Persistence.Repositories;

public class ProductQueryRepository : IProductQueryRepository
{
    private readonly EShopDbContext _context;

    public ProductQueryRepository(EShopDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto?> GetByIdAsync(ProductId id)
    {
        return await _context.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title.Value,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                LastModified = p.LastModified,
                LastModifiedBy = p.LastModifiedBy
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedResult<ProductDto>> GetProductsAsync(PaginationRequest pagination)
    {
        var query = _context.Products.AsNoTracking();
        var count = await query.CountAsync();

        var products = await query
            .OrderBy(p => p.Title.Value)
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title.Value,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                LastModified = p.LastModified,
                LastModifiedBy = p.LastModifiedBy
            })
            .ToListAsync();

        return new PaginatedResult<ProductDto>(
            pagination.PageIndex,
            pagination.PageSize,
            count,
            products);
    }

    public async Task<bool> ExistsAsync(ProductId id)
    {
        return await _context.Products
            .AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(Money minPrice, Money maxPrice)
    {
        return await _context.Products.AsNoTracking()
            .Where(p => p.Price.Amount >= minPrice.Amount
                    && p.Price.Amount <= maxPrice.Amount
                    && p.Price.Currency == minPrice.Currency)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title.Value,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                LastModified = p.LastModified,
                LastModifiedBy = p.LastModifiedBy
            })
            .ToListAsync();
    }
}
