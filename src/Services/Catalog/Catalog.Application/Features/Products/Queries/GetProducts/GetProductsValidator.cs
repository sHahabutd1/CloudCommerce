using FluentValidation;

namespace Catalog.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsValidator
    : AbstractValidator<GetProductsQuery>
{
    public GetProductsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Search)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Search));

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPrice.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x)
            .Must(x =>
                !x.MinPrice.HasValue ||
                !x.MaxPrice.HasValue ||
                x.MinPrice <= x.MaxPrice)
            .WithMessage(
                "Minimum price must be less than or equal to maximum price.");

        RuleFor(x => x.SortBy)
            .Must(BeAllowedSortField)
            .WithMessage(
                "SortBy must be one of: name, price, id.");

        RuleFor(x => x.SortDirection)
            .Must(BeAllowedSortDirection)
            .WithMessage(
                "SortDirection must be asc or desc.");

        RuleFor(x => x.Page)
            .InclusiveBetween(1, 10_000);
    }

    private static bool BeAllowedSortField(string? value)
    {
        return value is not null &&
            new[] { "name", "price", "id" }
                .Contains(
                    value,
                    StringComparer.OrdinalIgnoreCase);
    }

    private static bool BeAllowedSortDirection(string? value)
    {
        return value is not null &&
            new[] { "asc", "desc" }
                .Contains(
                    value,
                    StringComparer.OrdinalIgnoreCase);
    }
}