namespace EShop.Domain.Events;

public class OrderConfirmedEvent : DomainEvent
{
    public Order Order { get; }

    public OrderConfirmedEvent(Order order)
    {
        Order = order;
    }
}
