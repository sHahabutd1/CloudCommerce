using Catalog.Application.Common.Models;
using Catalog.Application.Contracts;
using Catalog.Application.Features.Products.Queries.GetProducts;
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


    public async Task<PagedResult<Product>> GetPagedAsync(
    ProductQueryParameters parameters,
    CancellationToken cancellationToken)
    {
        IQueryable<Product> query =
            _context.Products.AsNoTracking();

        // 1. Search
        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var search = parameters.Search.Trim();

            query = query.Where(
                p => p.Name.Contains(search) ||
                     p.Description.Contains(search));
        }

        // 2. Price filtering
        if (parameters.MinPrice.HasValue)
        {
            query = query.Where(
                p => p.Price >= parameters.MinPrice.Value);
        }

        if (parameters.MaxPrice.HasValue)
        {
            query = query.Where(
                p => p.Price <= parameters.MaxPrice.Value);
        }

        // 3. Count filtered records before pagination
        var totalCount =
            await query.CountAsync(cancellationToken);

        // 4. Apply allowlisted sorting
        var descending =
            string.Equals(
                parameters.SortDirection,
                "desc",
                StringComparison.OrdinalIgnoreCase);

        IOrderedQueryable<Product> orderedQuery =
            parameters.SortBy.ToLowerInvariant() switch
            {
                "name" => descending
                    ? query.OrderByDescending(p => p.Name)
                        .ThenBy(p => p.Id)
                    : query.OrderBy(p => p.Name)
                        .ThenBy(p => p.Id),

                "price" => descending
                    ? query.OrderByDescending(p => p.Price)
                        .ThenBy(p => p.Id)
                    : query.OrderBy(p => p.Price)
                        .ThenBy(p => p.Id),

                "id" => descending
                    ? query.OrderByDescending(p => p.Id)
                    : query.OrderBy(p => p.Id),

                _ => throw new InvalidOperationException(
                    "Unsupported product sort field.")
            };

        // 5. Calculate offset safely
        var skip = (int)(
            ((long)parameters.Page - 1) *
            parameters.PageSize);

        // 6. Execute pagination in PostgreSQL
        var products =
            await orderedQuery
                .Skip(skip)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

        return new PagedResult<Product>
        {
            Items = products,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };
    }


    //public async Task<List<Product>> GetAllAsync(
    //    CancellationToken cancellationToken)
    //{

    //    return await _context.Products
    //        .AsNoTracking()
    //        .ToListAsync(cancellationToken);
    //}


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