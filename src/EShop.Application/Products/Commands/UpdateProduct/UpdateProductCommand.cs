namespace EShop.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : ICommand
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public decimal Price { get; init; }
    public string Currency { get; init; } = "USD";
}
