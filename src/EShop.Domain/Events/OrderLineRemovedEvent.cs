namespace EShop.Domain.Events;

public class OrderLineRemovedEvent : DomainEvent
{
    public Order Order { get; }
    public OrderLine OrderLine { get; }

    public OrderLineRemovedEvent(Order order, OrderLine orderLine)
    {
        Order = order;
        OrderLine = orderLine;
    }
}
