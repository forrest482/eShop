namespace EShop.Application.Products.Abstractions;


public interface IProductQueryRepository : IQueryRepository
{
    Task<PaginatedResult<ProductDto>> GetProductsAsync(PaginationRequest pagination);
    Task<ProductDto?> GetByIdAsync(ProductId id);
    Task<bool> ExistsAsync(ProductId id);
    Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(Money minPrice, Money maxPrice);
}

