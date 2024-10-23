namespace EShop.Domain.Models;
public class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderLine> _orderLines = new();
    public IReadOnlyList<OrderLine> OrderLines => _orderLines.AsReadOnly();

    public OrderStatus Status { get; private set; }
    public Money TotalPrice => CalculateTotalPrice();


    private Order() // for EF Core

    {
    }

    private Order(OrderId id)
    {
        Id = id;
        Status = OrderStatus.Pending;
    }

    public static Order Create(OrderId id)
    {
        var order = new Order(id);
        order.AddDomainEvent(new OrderCreatedEvent(order));
        return order;
    }

    public void ConfirmOrder()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Can only confirm pending orders.");

        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderConfirmedEvent(this));
    }

    public void CompleteOrder()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException("Can only complete confirmed orders.");

        Status = OrderStatus.Completed;
        AddDomainEvent(new OrderCompletedEvent(this));
    }

    public void CancelOrder()
    {
        if (Status == OrderStatus.Completed)
            throw new DomainException("Cannot cancel completed orders.");

        Status = OrderStatus.Cancelled;
        AddDomainEvent(new OrderCancelledEvent(this));
    }

    public void AddOrderLine(ProductId productId, Money unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        var orderLine = new OrderLine(OrderLineId.Of(Guid.NewGuid()), Id, productId, unitPrice, quantity);
        _orderLines.Add(orderLine);

        AddDomainEvent(new OrderLineAddedEvent(this, orderLine));
    }

    public void RemoveOrderLine(OrderLineId orderLineId)
    {
        var orderLine = _orderLines.FirstOrDefault(x => x.Id == orderLineId);
        if (orderLine == null)
            throw new DomainException("Order line not found.");

        _orderLines.Remove(orderLine);
        AddDomainEvent(new OrderLineRemovedEvent(this, orderLine));
    }

    public void UpdateOrderLineQuantity(OrderLineId orderLineId, int newQuantity)
    {
        if (newQuantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        var orderLine = _orderLines.FirstOrDefault(x => x.Id == orderLineId);
        if (orderLine == null)
            throw new DomainException("Order line not found.");

        orderLine.UpdateQuantity(newQuantity);
        AddDomainEvent(new OrderLineUpdatedEvent(this, orderLine));
    }

    private Money CalculateTotalPrice()
    {
        return _orderLines.Aggregate(Money.Of(0), (total, line) => total + line.TotalPrice);
    }
}