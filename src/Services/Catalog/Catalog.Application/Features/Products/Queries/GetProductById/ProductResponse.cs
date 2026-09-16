namespace Catalog.Application.Features.Products.Queries.GetProductById;

public sealed class ProductResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = default!;

    public string Description { get; init; } = default!;

    public decimal Price { get; init; }
}