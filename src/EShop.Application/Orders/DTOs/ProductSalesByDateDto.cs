namespace EShop.Application.Orders.DTOs;

public record ProductSalesByDateDto
{
    public string ProductName { get; init; } = default!;
    public List<DailySale> Sales { get; init; } = new();

}

public record DailySale
{
    public DateTime Date { get; init; }
    public int Quantity { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = default!;
}
