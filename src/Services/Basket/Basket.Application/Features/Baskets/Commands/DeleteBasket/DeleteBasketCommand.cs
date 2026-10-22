using MediatR;

namespace Basket.Application.Features.Baskets.Commands.DeleteBasket;

public sealed record DeleteBasketCommand(Guid BasketId)
    : IRequest<Unit>;