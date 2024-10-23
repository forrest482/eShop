namespace EShop.Application.Orders.DTOs;

public class OrderDto
{
    public Guid Id { get; init; }
    public string Status { get; init; } = default!;
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = default!;
    public List<OrderLineDto> OrderLines { get; init; } = new();
    public DateTime? CreatedAt { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime? LastModified { get; init; }
    public string? LastModifiedBy { get; init; }
}