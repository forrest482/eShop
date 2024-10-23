namespace EShop.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
