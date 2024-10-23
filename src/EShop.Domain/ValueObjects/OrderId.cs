namespace EShop.Domain.ValueObjects;
public record OrderId : StronglyTypedId<Guid>
{
    private OrderId(Guid value) : base(value) { }

    public static OrderId Of(Guid value)
    {
        return new OrderId(value);
    }
}
