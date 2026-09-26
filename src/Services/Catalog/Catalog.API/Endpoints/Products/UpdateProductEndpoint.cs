using Catalog.Application.Features.Products.Commands.UpdateProduct;
using MediatR;

namespace Catalog.API.Endpoints.Products;

public static class UpdateProductEndpoint
{
    public static IEndpointRouteBuilder
        MapUpdateProductEndpoint(
            this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/products/{id:guid}",
            async (
                Guid id,
                UpdateProductRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command =
                    new UpdateProductCommand(
                        id,
                        request.Name,
                        request.Description,
                        request.Price);

                await sender.Send(
                    command,
                    cancellationToken);

                return Results.NoContent();
            })
            .WithName("UpdateProduct")
            .WithTags("Products");

        return app;
    }
}