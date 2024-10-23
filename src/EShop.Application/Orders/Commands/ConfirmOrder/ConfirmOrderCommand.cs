namespace EShop.Application.Orders.Commands.ConfirmOrder;

public record ConfirmOrderCommand(Guid OrderId) : ICommand;
