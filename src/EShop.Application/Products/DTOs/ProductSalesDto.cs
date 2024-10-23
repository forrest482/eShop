namespace EShop.Application.Products.DTOs;

public record ProductSalesDto
{
    public string ProductName { get; init; } = default!;
    public decimal TotalAmount { get; init; }
    public int Quantity { get; init; }
    public DateTime Date { get; init; }
}
