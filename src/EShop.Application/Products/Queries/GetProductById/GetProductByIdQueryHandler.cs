using EShop.Application.Products.Abstractions;

namespace EShop.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductQueryRepository _repository;

    public GetProductByIdQueryHandler(IProductQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(ProductId.Of(request.Id));
        if (product == null)
            throw new NotFoundException($"Product with ID {request.Id} not found");

        return product;
    }
}
