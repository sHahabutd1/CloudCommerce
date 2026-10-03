namespace Catalog.API.Endpoints.Products;

public static class ProductEndpointExtensions
{
    public static IEndpointRouteBuilder MapProductEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapCreateProductEndpoint();

        app.MapGetProductsEndpoint();

        app.MapGetProductByIdEndpoint();

        app.MapUpdateProductEndpoint();

        app.MapDeleteProductEndpoint();

        return app;
    }
}