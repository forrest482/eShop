namespace EShop.Domain.Events;

public class OrderCancelledEvent : DomainEvent
{
    public Order Order { get; }

    public OrderCancelledEvent(Order order)
    {
        Order = order;
    }
}
