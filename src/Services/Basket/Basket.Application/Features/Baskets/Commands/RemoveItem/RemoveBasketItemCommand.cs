using MediatR;

namespace Basket.Application.Features.Baskets.Commands.RemoveItem;

public sealed record RemoveBasketItemCommand(
    Guid BasketId,
    Guid ProductId) : IRequest<Unit>;