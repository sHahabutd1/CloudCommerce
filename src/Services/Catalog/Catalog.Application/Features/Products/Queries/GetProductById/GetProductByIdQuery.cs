using MediatR;

namespace Catalog.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id)
    : IRequest<ProductResponse>;