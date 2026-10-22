using Basket.Application.Contracts;
using Basket.Domain.Entities;
using MediatR;

namespace Basket.Application.Features.Baskets.Commands.AddItem;

public sealed class AddBasketItemHandler(
    IBasketRepository repository)
    : IRequestHandler<AddBasketItemCommand>
{
    public async Task Handle(
        AddBasketItemCommand request,
        CancellationToken cancellationToken)
    {
        if (request.BasketId == Guid.Empty)
            throw new ArgumentException("Invalid basket ID.");

        if (request.ProductId == Guid.Empty)
            throw new ArgumentException("Invalid product ID.");

        if (request.Quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(request.Quantity));

        var basket = await repository.GetAsync(
            request.BasketId, cancellationToken);

        basket ??= new ShoppingBasket(request.BasketId, null);

        basket.AddItem(request.ProductId, request.Quantity);

        await repository.SaveAsync(basket, cancellationToken);
    }
}