using EShop.Application.Products.Abstractions;

namespace EShop.Application.Products.Queries.GetProductsByPriceRange;

public class GetProductsByPriceRangeQueryHandler
    : IQueryHandler<GetProductsByPriceRangeQuery, IEnumerable<ProductDto>>
{
    private readonly IProductQueryRepository _repository;

    public GetProductsByPriceRangeQueryHandler(IProductQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(
        GetProductsByPriceRangeQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetProductsByPriceRangeAsync(
            Money.Of(request.MinPrice),
            Money.Of(request.MaxPrice)
        );
    }
}
