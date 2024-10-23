namespace EShop.Application.Orders.Queries.GetProductSalesCount;

public record GetProductSalesCountQuery : IQuery<List<ProductSalesCountDto>>
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
