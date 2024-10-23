using EShop.Application.Products.Abstractions;

namespace EShop.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler
    : IQueryHandler<GetProductsQuery, PaginatedResult<ProductDto>>
{
    private readonly IProductQueryRepository _repository;

    public GetProductsQueryHandler(IProductQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetProductsAsync(request.Pagination);
    }
}
