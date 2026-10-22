using MediatR;

namespace Basket.Application.Features.Baskets.Commands.AddItem;

public sealed record AddBasketItemCommand(
    Guid BasketId,
    Guid ProductId,
    int Quantity) : IRequest;