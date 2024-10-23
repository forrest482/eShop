namespace EShop.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IQuery<PaginatedResult<ProductDto>>
{
    public PaginationRequest Pagination { get; init; } = new();
}
