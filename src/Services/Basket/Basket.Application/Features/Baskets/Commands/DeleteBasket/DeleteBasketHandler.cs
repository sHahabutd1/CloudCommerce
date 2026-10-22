using Basket.Application.Contracts;
using MediatR;

namespace Basket.Application.Features.Baskets.Commands.DeleteBasket;

public sealed class DeleteBasketHandler(IBasketRepository repository)
    : IRequestHandler<DeleteBasketCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteBasketCommand request,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(
            request.BasketId, cancellationToken);

        return Unit.Value;
    }
}