namespace EShop.Application.Orders.DTOs;

public record CreateOrderLineDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}
