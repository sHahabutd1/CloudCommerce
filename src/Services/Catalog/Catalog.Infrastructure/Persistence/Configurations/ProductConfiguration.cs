using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Catalog.Infrastructure.Persistence.Configurations;


public class ProductConfiguration
    : IEntityTypeConfiguration<Product>
{

    public void Configure(
        EntityTypeBuilder<Product> builder)
    {

        builder.HasKey(x => x.Id);


        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();


        builder.Property(x => x.Price)
            .HasPrecision(18, 2);


        builder.Property(x => x.Description)
            .HasMaxLength(1000);


        builder.HasIndex(p => new { p.Name, p.Id })
                .HasDatabaseName("IX_Products_Name_Id");

        builder.HasIndex(p => new { p.Price, p.Id })
                .HasDatabaseName("IX_Products_Price_Id");
    }
}