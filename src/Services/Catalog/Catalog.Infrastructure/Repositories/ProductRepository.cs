using Catalog.Application.Contracts;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Catalog.Infrastructure.Repositories;


public class ProductRepository
    : IProductRepository
{

    private readonly CatalogDbContext _context;


    public ProductRepository(
        CatalogDbContext context)
    {
        _context = context;
    }



    public async Task<Product> AddAsync(
        Product product,
        CancellationToken cancellationToken)
    {

        _context.Products.Add(product);

        await _context.SaveChangesAsync(
            cancellationToken);

        return product;
    }



    public async Task<List<Product>> GetAllAsync(
        CancellationToken cancellationToken)
    {

        return await _context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }


    public async Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }
}