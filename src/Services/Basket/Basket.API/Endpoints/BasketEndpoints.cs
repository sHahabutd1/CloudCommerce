using Basket.Application.Features.Baskets.Commands.AddItem;
using Basket.Application.Features.Baskets.Commands.DeleteBasket;
using Basket.Application.Features.Baskets.Commands.RemoveItem;
using Basket.Application.Features.Baskets.Commands.UpdateQuantity;
using Basket.Application.Features.Baskets.Queries.GetBasket;
using MediatR;

namespace Basket.API.Endpoints;

public static class BasketEndpoints
{
    public static IEndpointRouteBuilder MapBasketEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/baskets")
            .WithTags("Baskets");

        group.MapGet("/{basketId:guid}",
            async (Guid basketId, ISender sender,
                CancellationToken ct) =>
            {
                var basket = await sender.Send(
                    new GetBasketQuery(basketId), ct);

                return basket is null
                    ? Results.NotFound()
                    : Results.Ok(basket);
            });

        group.MapPost("/{basketId:guid}/items",
            async (Guid basketId, AddBasketItemRequest request,
                ISender sender, CancellationToken ct) =>
            {
                await sender.Send(
                    new AddBasketItemCommand(
                        basketId, request.ProductId, request.Quantity), ct);

                return Results.NoContent();
            });

        group.MapPut("/{basketId:guid}/items/{productId:guid}",
            async (Guid basketId, Guid productId,
                UpdateBasketItemRequest request,
                ISender sender, CancellationToken ct) =>
            {
                await sender.Send(
                    new UpdateBasketItemQuantityCommand(
                        basketId, productId, request.Quantity), ct);

                return Results.NoContent();
            });

        group.MapDelete("/{basketId:guid}/items/{productId:guid}",
            async (Guid basketId, Guid productId,
                ISender sender, CancellationToken ct) =>
            {
                await sender.Send(
                    new RemoveBasketItemCommand(basketId, productId), ct);

                return Results.NoContent();
            });

        group.MapDelete("/{basketId:guid}",
            async (Guid basketId, ISender sender,
                CancellationToken ct) =>
            {
                await sender.Send(new DeleteBasketCommand(basketId), ct);
                return Results.NoContent();
            });

        return app;
    }
}

public sealed record AddBasketItemRequest(Guid ProductId, int Quantity);

public sealed record UpdateBasketItemRequest(int Quantity);