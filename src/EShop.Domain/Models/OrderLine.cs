namespace EShop.Domain.Models;
public class OrderLine : Entity<OrderLineId>
{
    public OrderId OrderId { get; private set; }
    public ProductId ProductId { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public Money TotalPrice => UnitPrice * Quantity;

    private OrderLine() // for EF Core
    {
    }

    internal OrderLine(OrderLineId id, OrderId orderId, ProductId productId, Money unitPrice, int quantity)
       
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        Quantity = newQuantity;
    }
}
