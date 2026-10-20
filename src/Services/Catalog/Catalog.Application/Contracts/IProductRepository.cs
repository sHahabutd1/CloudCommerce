using Catalog.Application.Common.Models;
using Catalog.Application.Features.Products.Queries.GetProducts;
using Catalog.Domain.Entities;


namespace Catalog.Application.Contracts;


public interface IProductRepository
{

    Task<Product> AddAsync(
        Product product,
        CancellationToken cancellationToken);


    //Task<List<Product>> GetAllAsync(
    //    CancellationToken cancellationToken);

    Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Product>> GetPagedAsync(
        ProductQueryParameters parameters,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);


    void Delete(Product product);
}