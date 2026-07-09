namespace Catalog.Application.Features.Products.Dtos;


public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock
);
