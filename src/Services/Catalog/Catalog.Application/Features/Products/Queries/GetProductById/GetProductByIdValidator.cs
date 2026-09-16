using Catalog.Application.Features.Products.Queries.GetProductById;
using FluentValidation;

public class GetProductByIdValidator
    : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}