namespace EShop.Application.Orders.DTOs;

public record ProductSalesCountDto
{
    public string ProductName { get; init; } = default!;
    public int TotalCount { get; init; }
}
