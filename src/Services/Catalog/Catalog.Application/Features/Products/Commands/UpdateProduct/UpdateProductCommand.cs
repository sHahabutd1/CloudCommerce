using MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price)
    : IRequest;