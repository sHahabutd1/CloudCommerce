using Catalog.Application.Features.Products.Queries.GetProductById;
using MediatR;

namespace Catalog.API.Endpoints.Products;

public static class GetProductByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetProductByIdEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/products/{id:guid}",
            async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var product =
                    await sender.Send(
                        new GetProductByIdQuery(id),
                        cancellationToken);

                return Results.Ok(product);
            })
            .WithName("GetProductById")
            .WithTags("Products");

        return app;
    }
}