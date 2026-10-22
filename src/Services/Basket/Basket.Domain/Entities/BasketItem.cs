namespace Basket.Domain.Entities;

public sealed class BasketItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    private BasketItem()
    {
    }

    public BasketItem(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException(
                "Product ID cannot be empty.", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be greater than zero.");

        ProductId = productId;
        Quantity = quantity;
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be greater than zero.");

        Quantity = quantity;
    }
}