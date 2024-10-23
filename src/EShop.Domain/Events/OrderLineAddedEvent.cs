namespace EShop.Domain.Events;

public class OrderLineAddedEvent : DomainEvent
{
    public Order Order { get; }
    public OrderLine OrderLine { get; }

    public OrderLineAddedEvent(Order order, OrderLine orderLine)
    {
        Order = order;
        OrderLine = orderLine;
    }
}
