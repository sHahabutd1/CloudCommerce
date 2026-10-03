using Catalog.Application.Contracts;
using Catalog.Application.Exceptions;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.DeleteProduct;

internal sealed class DeleteProductHandler
    : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;

    public DeleteProductHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        DeleteProductCommand request,
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

        _repository.Delete(product);

        await _repository.SaveChangesAsync(
            cancellationToken);
    }
}