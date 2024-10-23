namespace EShop.Domain.ValueObjects;
public record ProductId : StronglyTypedId<Guid>
{
    private ProductId(Guid value) : base(value) { }

    public static ProductId Of(Guid value)
    {
        return new ProductId(value);
    }
}
