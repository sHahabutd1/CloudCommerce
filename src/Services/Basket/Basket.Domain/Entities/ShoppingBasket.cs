using System.Text.Json.Serialization;

namespace Basket.Domain.Entities;

public sealed class ShoppingBasket
{
    public Guid Id { get; private set; }

    public List<BasketItem> Items { get; private set; } = [];

    private ShoppingBasket()
    {
    }

    [JsonConstructor]
    public ShoppingBasket(Guid id, List<BasketItem>? items)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Basket ID cannot be empty.", nameof(id));

        Id = id;
        Items = items ?? [];
    }

    public void AddItem(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        var existingItem = Items.FirstOrDefault(
            item => item.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.UpdateQuantity(checked(existingItem.Quantity + quantity));
            return;
        }

        Items.Add(new BasketItem(productId, quantity));
    }

    public void UpdateQuantity(Guid productId, int quantity)
    {
        var item = Items.FirstOrDefault(item => item.ProductId == productId);

        if (item is null)
            throw new InvalidOperationException("Product does not exist in this basket.");

        item.UpdateQuantity(quantity);
    }

    public void RemoveItem(Guid productId)
    {
        Items.RemoveAll(item => item.ProductId == productId);
    }
}