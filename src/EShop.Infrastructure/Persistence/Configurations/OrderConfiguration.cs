using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
              .HasConversion(
                   id => id.Value,
                   value => OrderId.Of(value))
              .ValueGeneratedNever()
              .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<string>();

        builder.OwnsOne(e => e.TotalPrice, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 2);

            priceBuilder.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3);
        });

        builder.HasMany(e => e.OrderLines)
            .WithOne()
            .HasForeignKey(e => e.OrderId);

        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.TotalPrice);

    }
}
