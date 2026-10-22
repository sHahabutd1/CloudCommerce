using Basket.Application.Contracts;
using MediatR;

namespace Basket.Application.Features.Baskets.Commands.UpdateQuantity;

public sealed class UpdateBasketItemQuantityHandler(
    IBasketRepository repository)
    : IRequestHandler<UpdateBasketItemQuantityCommand, Unit>
{
    public async Task<Unit> Handle(
        UpdateBasketItemQuantityCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(request.Quantity));

        var basket = await repository.GetAsync(
            request.BasketId, cancellationToken)
            ?? throw new KeyNotFoundException("Basket not found.");

        basket.UpdateQuantity(request.ProductId, request.Quantity);

        await repository.SaveAsync(basket, cancellationToken);

        return Unit.Value;
    }
}