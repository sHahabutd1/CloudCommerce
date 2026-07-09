using Catalog.Application.Features.Products.Dtos;
using MediatR;


namespace Catalog.Application.Features.Products.Queries.GetProducts;


public record GetProductsQuery()
    : IRequest<List<ProductDto>>;
