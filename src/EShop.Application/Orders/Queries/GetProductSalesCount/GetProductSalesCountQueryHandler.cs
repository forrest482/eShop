using EShop.Application.Orders.Abstractions;

namespace EShop.Application.Orders.Queries.GetProductSalesCount;

public class GetProductSalesCountQueryHandler
    : IQueryHandler<GetProductSalesCountQuery, List<ProductSalesCountDto>>
{
    private readonly IOrderQueryRepository _repository;

    public GetProductSalesCountQueryHandler(IOrderQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductSalesCountDto>> Handle(
        GetProductSalesCountQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetProductSalesInPeriodAsync(
            request.StartDate,
            request.EndDate);
    }
}
