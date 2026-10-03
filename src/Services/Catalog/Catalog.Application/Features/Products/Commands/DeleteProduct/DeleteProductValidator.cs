using FluentValidation;

namespace Catalog.Application.Features.Products.Commands.DeleteProduct;

public sealed class DeleteProductValidator
    : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}