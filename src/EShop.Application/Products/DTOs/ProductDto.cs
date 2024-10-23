namespace EShop.Application.Products.DTOs;

public record ProductDto
{
    public ProductId Id { get; init; } = default!;
    public string Title { get; init; } = default!;
    public Money Price { get; init; } = default!;
    public DateTime? CreatedAt { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime? LastModified { get; init; }
    public string? LastModifiedBy { get; init; }
}
