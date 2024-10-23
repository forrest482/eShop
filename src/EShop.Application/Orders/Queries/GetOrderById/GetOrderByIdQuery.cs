namespace EShop.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderDto>;
