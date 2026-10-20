namespace Catalog.Application.Features.Products.Queries.GetProducts;

public sealed record ProductQueryParameters(
    int Page,
    int PageSize,
    string? Search,
    decimal? MinPrice,
    decimal? MaxPrice,
    string SortBy,
    string SortDirection);