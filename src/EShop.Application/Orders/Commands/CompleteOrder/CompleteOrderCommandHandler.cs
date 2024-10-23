namespace EShop.Application.Orders.Commands.CompleteOrder;

public class CompleteOrderCommandHandler : ICommandHandler<CompleteOrderCommand>
{
    private readonly IOrderCommandRepository _repository;

    public CompleteOrderCommandHandler(IOrderCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(OrderId.Of(request.OrderId));
        if (order == null)
            throw new NotFoundException($"Order {request.OrderId} not found");

        order.CompleteOrder();
        await _repository.UpdateAsync(order);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
