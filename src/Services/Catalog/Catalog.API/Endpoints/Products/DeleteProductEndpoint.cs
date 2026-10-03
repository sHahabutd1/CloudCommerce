using Catalog.Application.Features.Products.Commands.DeleteProduct;
using MediatR;

namespace Catalog.API.Endpoints.Products;

public static class DeleteProductEndpoint
{
    public static IEndpointRouteBuilder
        MapDeleteProductEndpoint(
            this IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/products/{id:guid}",
            async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(
                    new DeleteProductCommand(id),
                    cancellationToken);

                return Results.NoContent();
            })
            .WithName("DeleteProduct")
            .WithTags("Products");

        return app;
    }
}