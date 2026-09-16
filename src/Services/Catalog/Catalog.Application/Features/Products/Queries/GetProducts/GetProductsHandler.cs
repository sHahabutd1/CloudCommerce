using Catalog.Application.Contracts;
using Catalog.Application.Features.Products.Dtos;
using MediatR;


namespace Catalog.Application.Features.Products.Queries.GetProducts;


internal sealed class GetProductsHandler
    : IRequestHandler<GetProductsQuery, List<ProductDto>>
{

    private readonly IProductRepository _repository;


    public GetProductsHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }



    public async Task<List<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {

        var products =
            await _repository.GetAllAsync(
                cancellationToken);


        return products
            .Select(x =>
                new ProductDto(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Price,
                    x.Stock))
            .ToList();

    }

}
