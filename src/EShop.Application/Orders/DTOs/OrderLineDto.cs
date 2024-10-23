namespace EShop.Application.Orders.DTOs;

public class OrderLineDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = default!;
    public decimal UnitPrice { get; init; }
    public string Currency { get; init; } = default!;
    public int Quantity { get; init; }
    public decimal TotalPrice { get; init; }
}