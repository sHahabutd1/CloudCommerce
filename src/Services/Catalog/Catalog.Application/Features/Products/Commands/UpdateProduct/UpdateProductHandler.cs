using Catalog.Application.Contracts;
using Catalog.Application.Exceptions;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductHandler
    : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _repository;

    public UpdateProductHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        UpdateProductCommand request,
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

        product.Update(
            request.Name,
            request.Description,
            request.Price);

        await _repository.SaveChangesAsync(
            cancellationToken);
    }
}