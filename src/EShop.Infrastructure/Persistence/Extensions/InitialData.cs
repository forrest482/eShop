namespace EShop.Infrastructure.Persistence.Extensions;

internal static class InitialData
{
    private static readonly DateTime _now = DateTime.UtcNow;

    public static IEnumerable<Product> Products
    {
        get
        {
            var products = new[]
            {
                (title: "iPhone 14 Pro", price: 999.99m),
                (title: "Samsung Galaxy S23", price: 899.99m),
                (title: "Google Pixel 7", price: 699.99m),
                (title: "OnePlus 11", price: 699.99m),
                (title: "MacBook Pro 16", price: 2499.99m),
                (title: "Dell XPS 15", price: 1899.99m),
                (title: "iPad Air", price: 599.99m),
                (title: "AirPods Pro", price: 249.99m),
                (title: "Samsung Galaxy Watch 5", price: 279.99m),
                (title: "Sony WH-1000XM4", price: 349.99m)
            };

            return products.Select(p => CreateProduct(p.title, p.price)).ToList();
        }
    }

    private static Product CreateProduct(string title, decimal price)
    {
        var id = ProductId.Of(Guid.NewGuid());
        var titleVO = ProductTitle.Of(title);
        var priceVO = Money.Of(price, "USD");

        var product = Product.Create(id, titleVO, priceVO);

        var createdAt = _now.AddDays(-30);
        product.CreatedAt = createdAt;
        product.CreatedBy = "system";
        product.LastModified = createdAt;
        product.LastModifiedBy = "system";

        return product;
    }

    public static IEnumerable<Order> Orders
    {
        get
        {
            var orders = new List<Order>();
            var random = new Random();

            for (int i = 0; i < 30; i++)
            {
                var daysAgo = random.Next(0, 10);
                var orderDate = _now.AddDays(-daysAgo);
                var order = CreateOrder(orderDate, random);
                orders.Add(order);
            }

            return orders;
        }
    }

    private static Order CreateOrder(DateTime orderDate, Random random)
    {
        var orderId = OrderId.Of(Guid.NewGuid());
        var order = Order.Create(orderId);

        order.CreatedAt = orderDate;
        order.CreatedBy = "system";
        order.LastModified = orderDate;
        order.LastModifiedBy = "system";


        var statusRandom = random.NextDouble();
        if (statusRandom < 0.7) // 70% completed
        {
            order.ConfirmOrder();
            order.CompleteOrder();
        }
        else if (statusRandom < 0.9) // 20% confirmed
        {
            order.ConfirmOrder();
        }

        return order;
    }

    public static void AddOrderLines(Order order, IEnumerable<Product> availableProducts, Random random)
    {
        var numberOfLines = random.Next(1, 5);
        var shuffledProducts = availableProducts.OrderBy(x => random.Next()).Take(numberOfLines).ToList();

        foreach (var product in shuffledProducts)
        {
            var quantity = random.Next(1, 5);
            var unitPrice = Money.Of(product.Price.Amount, product.Price.Currency);
            order.AddOrderLine(product.Id, unitPrice, quantity);
        }
    }
}