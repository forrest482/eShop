
namespace EShop.Domain.Models;
public class Product : AggregateRoot<ProductId>
{
    public ProductTitle Title { get; private set; } = default!;
    public Money Price { get; private set; }

    private Product() // for EF Core
    {
    }

    private Product(ProductId id, ProductTitle title, Money price)
    {
        Id = id;
        Title = title;
        Price = price;
    }

    public static Product Create(ProductId id, ProductTitle title, Money price)
    {
        var product = new Product(id, title, price);
        product.AddDomainEvent(new ProductCreatedEvent(product));
        return product;
    }

    public void Update(ProductTitle newTitle, Money newPrice)
    {
        Title = newTitle;
        Price = newPrice;
        AddDomainEvent(new ProductUpdatedEvent(this));
    }

}
