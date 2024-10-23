namespace EShop.Application.Orders.Abstractions;

public interface IOrderQueryRepository : IQueryRepository
{
    Task<OrderDto?> GetByIdAsync(OrderId id);
    Task<PaginatedResult<OrderDto>> GetOrdersAsync(PaginationRequest pagination);
    Task<List<ProductSalesByDateDto>> GetProductSalesLastWeekAsync(DateTime endDate);
    Task<List<ProductSalesCountDto>> GetProductSalesInPeriodAsync(DateTime startDate, DateTime endDate);
}
