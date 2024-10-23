namespace EShop.Domain.Events;

public class OrderCompletedEvent : DomainEvent
{
    public Order Order { get; }

    public OrderCompletedEvent(Order order)
    {
        Order = order;
    }
}
