namespace EShop.Application.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand>
{
    private readonly IOrderCommandRepository _repository;

    public CancelOrderCommandHandler(IOrderCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(OrderId.Of(request.OrderId));
        if (order == null)
            throw new NotFoundException($"Order {request.OrderId} not found");

        order.CancelOrder();
        await _repository.UpdateAsync(order);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
