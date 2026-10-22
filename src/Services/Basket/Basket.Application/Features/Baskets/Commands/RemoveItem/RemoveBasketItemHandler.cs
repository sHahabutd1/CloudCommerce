using Basket.Application.Contracts;
using MediatR;

namespace Basket.Application.Features.Baskets.Commands.RemoveItem;

public sealed class RemoveBasketItemHandler(
    IBasketRepository repository)
    : IRequestHandler<RemoveBasketItemCommand, Unit>
{
    public async Task<Unit> Handle(
        RemoveBasketItemCommand request,
        CancellationToken cancellationToken)
    {
        var basket = await repository.GetAsync(
            request.BasketId, cancellationToken)
            ?? throw new KeyNotFoundException("Basket not found.");

        basket.RemoveItem(request.ProductId);

        await repository.SaveAsync(basket, cancellationToken);

        return Unit.Value;
    }
}