using EShop.Application.Orders.Abstractions;

namespace EShop.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderQueryRepository _repository;

    public GetOrderByIdQueryHandler(IOrderQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(OrderId.Of(request.OrderId));
        if (order == null)
            throw new NotFoundException($"Order {request.OrderId} not found");

        return order;
    }
}

