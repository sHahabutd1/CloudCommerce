using Catalog.Application.Features.Products.Queries.GetProducts;
using MediatR;

namespace Catalog.API.Endpoints.Products;

public static class GetProductsEndpoint
{
    public static IEndpointRouteBuilder MapGetProductsEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/products",
            async (
            ISender sender,
            CancellationToken cancellationToken) =>
            {

                var products =
                    await sender.Send(
                        new GetProductsQuery(),
                        cancellationToken);


                return Results.Ok(products);

            })
            .WithName("GetProducts")
            .WithTags("Products");

        return app;
    }
}
