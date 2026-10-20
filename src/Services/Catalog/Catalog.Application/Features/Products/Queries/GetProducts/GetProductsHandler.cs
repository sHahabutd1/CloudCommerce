using Catalog.Application.Common.Models;
using Catalog.Application.Contracts;
using Catalog.Application.Features.Products.Models;
using MediatR;

namespace Catalog.Application.Features.Products.Queries.GetProducts;

internal sealed class GetProductsHandler
    : IRequestHandler<
        GetProductsQuery,
        PagedResult<ProductResponse>>
{
    private readonly IProductRepository _repository;

    public GetProductsHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ProductResponse>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var parameters =
    new ProductQueryParameters(
        request.Page,
        request.PageSize,
        request.Search,
        request.MinPrice,
        request.MaxPrice,
        request.SortBy,
        request.SortDirection);

        var result =
            await _repository.GetPagedAsync(
                parameters,
                cancellationToken);

        return new PagedResult<ProductResponse>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,

            Items = result.Items
                .Select(product =>
                    new ProductResponse
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price
                    })
                .ToList()
        };
    }
}