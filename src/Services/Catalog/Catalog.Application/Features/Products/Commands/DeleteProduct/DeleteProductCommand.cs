using MediatR;

namespace Catalog.Application.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id)
    : IRequest;