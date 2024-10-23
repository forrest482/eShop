namespace EShop.Domain.ValueObjects;
public record OrderLineId : StronglyTypedId<Guid>
{
    private OrderLineId(Guid value) : base(value) { }

    public static OrderLineId Of(Guid value)
    {
        return new OrderLineId(value);
    }
}
