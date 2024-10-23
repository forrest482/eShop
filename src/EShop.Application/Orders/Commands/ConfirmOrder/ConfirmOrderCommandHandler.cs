namespace EShop.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand>
{
    private readonly IOrderCommandRepository _repository;

    public ConfirmOrderCommandHandler(IOrderCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(OrderId.Of(request.OrderId));
        if (order == null)
            throw new NotFoundException($"Order {request.OrderId} not found");

        order.ConfirmOrder();
        await _repository.UpdateAsync(order);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

