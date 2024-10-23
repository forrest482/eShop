using EShop.Application.Products.Abstractions;

namespace EShop.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderCommandRepository _orderRepository;
    private readonly IProductQueryRepository _productRepository;

    public CreateOrderCommandHandler(
        IOrderCommandRepository orderRepository,
        IProductQueryRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(Guid.NewGuid());
        var order = Order.Create(orderId);

        foreach (var line in request.OrderLines)
        {
            var product = await _productRepository.GetByIdAsync(ProductId.Of(line.ProductId));
            if (product == null)
                throw new NotFoundException($"Product {line.ProductId} not found");

            order.AddOrderLine(product.Id, product.Price, line.Quantity);
        }

        await _orderRepository.AddAsync(order);
        await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id.Value;
    }
}

