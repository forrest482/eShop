namespace EShop.Application.Products.DTOs;

public record UpdateProductDto
{
    public string Title { get; init; } = default!;
    public decimal Price { get; init; }
}
