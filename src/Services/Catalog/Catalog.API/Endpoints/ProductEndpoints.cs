using Catalog.Application.Features.Products.Commands.CreateProduct;
using Catalog.Application.Features.Products.Queries.GetProducts;
using MediatR;


namespace Catalog.API.Endpoints;


public static class ProductEndpoints
{

    public static IEndpointRouteBuilder MapProductEndpoints(
        this IEndpointRouteBuilder app)
    {


        var group = app.MapGroup("/api/products")
            .WithTags("Products");


        group.MapPost(
            "/",
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
            .WithName("CreateProduct");

        group.MapGet(
            "/",
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
            .WithName("GetProducts");



        return app;

    }

}