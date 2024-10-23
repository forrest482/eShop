namespace EShop.Application.Products.DTOs;

public record CreateProductDto
{
    public string Title { get; init; } = default!;
    public decimal Price { get; init; }
}
