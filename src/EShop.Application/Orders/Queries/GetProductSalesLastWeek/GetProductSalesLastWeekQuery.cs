namespace EShop.Application.Orders.Queries.GetProductSalesLastWeek;

public record GetProductSalesLastWeekQuery(DateTime EndDate) : IQuery<List<ProductSalesByDateDto>>;
