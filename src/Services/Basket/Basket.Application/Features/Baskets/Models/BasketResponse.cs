namespace Basket.Application.Features.Baskets.Models;

public sealed record BasketResponse(
    Guid Id,
    IReadOnlyList<BasketItemResponse> Items);

public sealed record BasketItemResponse(
    Guid ProductId,
    int Quantity);