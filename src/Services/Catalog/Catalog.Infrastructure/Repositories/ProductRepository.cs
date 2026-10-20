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
            _context.Products
                .AsNoTracking();

        // Search
        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var search =
                parameters.Search.Trim();

            query = query.Where(
                p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
        }

        // Minimum price
        if (parameters.MinPrice.HasValue)
        {
            query = query.Where(
                p => p.Price >= parameters.MinPrice.Value);
        }

        // Maximum price
        if (parameters.MaxPrice.HasValue)
        {
            query = query.Where(
                p => p.Price <= parameters.MaxPrice.Value);
        }

        // Total count BEFORE pagination
        var totalCount =
            await query.CountAsync(
                cancellationToken);

        // Deterministic ordering
        query = query.OrderBy(
            p => p.Id);

        // Pagination
        var skip =
            (parameters.Page - 1)
            * parameters.PageSize;

        var products =
            await query
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