using System.Reflection;

namespace EShop.Infrastructure.Persistence;

public class EShopDbContext : DbContext, IUnitOfWork
{

    public EShopDbContext(
        DbContextOptions<EShopDbContext> options
       ) : base(options)
    {
       
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<Product> Products => Set<Product>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
