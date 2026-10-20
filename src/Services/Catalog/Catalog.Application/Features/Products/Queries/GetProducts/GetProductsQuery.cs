using Catalog.Application.Common.Models;
using Catalog.Application.Features.Products.Models;
using MediatR;

namespace Catalog.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string SortBy = "name",
    string SortDirection = "asc")
    : IRequest<PagedResult<ProductResponse>>;