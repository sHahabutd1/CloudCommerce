namespace Catalog.API.Endpoints.Products;

public sealed record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price);