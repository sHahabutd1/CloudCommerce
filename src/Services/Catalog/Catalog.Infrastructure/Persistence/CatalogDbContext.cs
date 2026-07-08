using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence;

public class CatalogDbContext : DbContext
{

    public CatalogDbContext(
        DbContextOptions<CatalogDbContext> options)
        : base(options)
    {

    }


    public DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CatalogDbContext).Assembly);
    }
}