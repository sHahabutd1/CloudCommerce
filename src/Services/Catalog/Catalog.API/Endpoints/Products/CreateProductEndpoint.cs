using Catalog.Application.Features.Products.Commands.CreateProduct;
using MediatR;

namespace Catalog.API.Endpoints.Products;

public static class CreateProductEndpoint
{
    public static IEndpointRouteBuilder MapCreateProductEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/products",
            async (
                CreateProductCommand command,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var id =
                    await sender.Send(
                        command,
                        cancellationToken);

                return Results.Created(
                    $"/api/products/{id}",
                    id);
            })
            .WithName("CreateProduct")
            .WithTags("Products");

        return app;
    }
}