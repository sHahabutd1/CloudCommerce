using Basket.Domain.Entities;

namespace Basket.Application.Contracts;

public interface IBasketRepository
{
    Task<ShoppingBasket?> GetAsync(
        Guid basketId,
        CancellationToken cancellationToken = default);

    Task SaveAsync(
        ShoppingBasket basket,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid basketId,
        CancellationToken cancellationToken = default);
}