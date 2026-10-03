using Catalog.Domain.Entities;


namespace Catalog.Application.Contracts;


public interface IProductRepository
{

    Task<Product> AddAsync(
        Product product,
        CancellationToken cancellationToken);


    Task<List<Product>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);


    void Delete(Product product);
}