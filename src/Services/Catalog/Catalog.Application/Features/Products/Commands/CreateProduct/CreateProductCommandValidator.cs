using FluentValidation;


namespace Catalog.Application.Features.Products.Commands.CreateProduct;


public class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{


    public CreateProductCommandValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);


        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);


        RuleFor(x => x.Price)
            .GreaterThan(0);


        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0);

    }

}