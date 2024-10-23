namespace EShop.Application.Orders.Commands.CompleteOrder;

public record CompleteOrderCommand(Guid OrderId) : ICommand;
