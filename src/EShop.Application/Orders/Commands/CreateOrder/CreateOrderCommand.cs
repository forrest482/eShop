namespace EShop.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand : ICommand<Guid>
{
    public List<CreateOrderLineDto> OrderLines { get; init; } = new();
}
