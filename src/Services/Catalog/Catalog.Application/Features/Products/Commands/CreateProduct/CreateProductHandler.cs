using Catalog.Application.Contracts;
using Catalog.Domain.Entities;
using MediatR;


namespace Catalog.Application.Features.Products.Commands.CreateProduct;


public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, Guid>
{


    private readonly IProductRepository _repository;


    public CreateProductHandler(
        IProductRepository repository)
    {
        _repository = repository;
    }



    public async Task<Guid> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {


        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.Stock);


        await _repository.AddAsync(
            product,
            cancellationToken);


        return product.Id;
    }
}