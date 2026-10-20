using Catalog.Application.Features.Products.Queries.GetProducts;
using MediatR;

namespace Catalog.API.Endpoints.Products;

public static class GetProductsEndpoint
{
    public static IEndpointRouteBuilder
        MapGetProductsEndpoint(
            this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/products",
            async (
                [AsParameters] GetProductsRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query =
                    new GetProductsQuery(
                        request.Page,
                        request.PageSize,
                        request.Search,
                        request.MinPrice,
                        request.MaxPrice);

                var result =
                    await sender.Send(
                        query,
                        cancellationToken);

                return Results.Ok(result);
            })
            .WithName("GetProducts")
            .WithTags("Products");

        return app;
    }
}