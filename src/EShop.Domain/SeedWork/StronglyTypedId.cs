namespace EShop.Domain.SeedWork;

public abstract record StronglyTypedId<T>  where T : notnull
{
    public T Value { get; }

    public StronglyTypedId(T value)
    {
        if (value is Guid guidValue && guidValue == Guid.Empty)
            throw new DomainException($"The {GetType().Name} cannot be empty.");

        Value = value;
    }

    public override string ToString() => Value.ToString()!;


}