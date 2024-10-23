using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EShop.Infrastructure.Persistence.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EShopDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<EShopDbContext>>();

        try
        {
            logger.LogInformation("Starting database initialization");

            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrated successfully");

            await SeedDataAsync(context, logger);

            logger.LogInformation("Database initialization completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    private static async Task SeedDataAsync(EShopDbContext context, ILogger logger)
    {
        try
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                if (!await context.Products.AnyAsync())
                {
                    logger.LogInformation("Seeding products");
                    var products = InitialData.Products.ToList();
                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Products seeded successfully");
                }

                if (!await context.Orders.AnyAsync())
                {
                    logger.LogInformation("Seeding orders");

                    // Get existing products
                    var availableProducts = await context.Products.AsNoTracking().ToListAsync();
                    var random = new Random();

                    // Create orders and add order lines
                    var orders = InitialData.Orders.ToList();
                    foreach (var order in orders)
                    {
                        InitialData.AddOrderLines(order, availableProducts, random);
                    }

                    await context.Orders.AddRangeAsync(orders);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Orders seeded successfully");
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during seeding, rolling back transaction");
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
}