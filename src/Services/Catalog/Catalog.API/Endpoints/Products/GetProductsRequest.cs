namespace Catalog.API.Endpoints.Products;

public sealed record GetProductsRequest(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null);