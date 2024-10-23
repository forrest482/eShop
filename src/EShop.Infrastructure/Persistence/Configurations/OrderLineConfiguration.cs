using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Infrastructure.Persistence.Configurations;

public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.HasKey(e => e.Id);


        builder.Property(e => e.Id)
               .HasConversion(
                    id => id.Value,
                    value => OrderLineId.Of(value))
               .ValueGeneratedNever()
               .IsRequired();

      
        // Foreign key for Order
        builder.Property(e => e.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.Of(value))
            .IsRequired();

        // Foreign key for Product
        builder.Property(e => e.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.Of(value))
            .IsRequired();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(ol => ol.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ComplexProperty(e => e.UnitPrice, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount)
                .HasColumnName("UnitPrice")
                .HasPrecision(18, 2)
                .IsRequired();

            moneyBuilder.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .HasDefaultValue("USD")
                .IsRequired();
        });


        builder.Property(e => e.Quantity);

        builder.Ignore(e => e.TotalPrice);

    }
}
