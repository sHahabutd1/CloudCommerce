using Catalog.Application.Contracts;
using Catalog.Application.Exceptions;
using MediatR;

namespace Catalog.Application.Features.Products.Queries.GetProductById;

internal sealed class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductResponse> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product =
            await _repository.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                $"Product '{request.Id}' was not found.");
        }

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
    }
}