namespace EShop.Domain.Events;

public class OrderLineUpdatedEvent : DomainEvent
{
    public Order Order { get; }
    public OrderLine OrderLine { get; }

    public OrderLineUpdatedEvent(Order order, OrderLine orderLine)
    {
        Order = order;
        OrderLine = orderLine;
    }
}
