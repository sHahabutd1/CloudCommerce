using MediatR;

namespace Basket.Application.Features.Baskets.Commands.UpdateQuantity;

public sealed record UpdateBasketItemQuantityCommand(
    Guid BasketId,
    Guid ProductId,
    int Quantity) : IRequest<Unit>;