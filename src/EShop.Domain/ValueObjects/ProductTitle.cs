namespace EShop.Domain.ValueObjects;
public record ProductTitle
{
    public string Value { get; }

    private ProductTitle(string value) => Value = value;

    public static ProductTitle Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Product title cannot be empty.");

        if (value.Length < 3 || value.Length > 100)
            throw new DomainException("Product title must be between 3 and 100 characters.");

        return new ProductTitle(value);
    }
}
