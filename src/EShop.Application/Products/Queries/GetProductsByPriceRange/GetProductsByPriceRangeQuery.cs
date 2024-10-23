namespace EShop.Application.Products.Queries.GetProductsByPriceRange;

public record GetProductsByPriceRangeQuery : IQuery<IEnumerable<ProductDto>>
{
    public decimal MinPrice { get; init; }
    public decimal MaxPrice { get; init; }
}
