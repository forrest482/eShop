using EShop.Application.Orders.Abstractions;

namespace EShop.Application.Orders.Queries.GetProductSalesLastWeek;

public class GetProductSalesLastWeekQueryHandler
    : IQueryHandler<GetProductSalesLastWeekQuery, List<ProductSalesByDateDto>>
{
    private readonly IOrderQueryRepository _repository;

    public GetProductSalesLastWeekQueryHandler(IOrderQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductSalesByDateDto>> Handle(
        GetProductSalesLastWeekQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetProductSalesLastWeekAsync(request.EndDate);
    }
}