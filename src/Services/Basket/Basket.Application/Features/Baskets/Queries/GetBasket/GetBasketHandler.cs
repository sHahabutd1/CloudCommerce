using Basket.Application.Contracts;
using Basket.Application.Features.Baskets.Models;
using MediatR;

namespace Basket.Application.Features.Baskets.Queries.GetBasket;

public sealed class GetBasketHandler(IBasketRepository repository)
    : IRequestHandler<GetBasketQuery, BasketResponse?>
{
    public async Task<BasketResponse?> Handle(
        GetBasketQuery request,
        CancellationToken cancellationToken)
    {
        var basket = await repository.GetAsync(
            request.BasketId, cancellationToken);

        return basket is null
            ? null
            : new BasketResponse(
                basket.Id,
                basket.Items.Select(item =>
                    new BasketItemResponse(
                        item.ProductId,
                        item.Quantity)).ToList());
    }
}