namespace EShop.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : ICommand<Guid>
{
    public string Title { get; init; } = default!;
    public decimal Price { get; init; }
    public string Currency { get; init; } = "USD";

}
