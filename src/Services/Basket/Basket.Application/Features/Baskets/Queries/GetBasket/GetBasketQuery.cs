using Basket.Application.Features.Baskets.Models;
using MediatR;

namespace Basket.Application.Features.Baskets.Queries.GetBasket;

public sealed record GetBasketQuery(Guid BasketId)
    : IRequest<BasketResponse?>;