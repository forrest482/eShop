namespace EShop.Domain.Repositories;

public interface IProductCommandRepository : ICommandRepository<Product>
{
    Task<Product> GetByIdAsync(ProductId id);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(ProductId id);
}
