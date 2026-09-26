namespace Catalog.Domain.Entities;
public class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }


    private Product()
    {
    }


    public Product(
        string name,
        string description,
        decimal price,
        int stock)
    {
        Id = Guid.NewGuid();

        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }


    public void Update(
        string name,
        string description,
        decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
    }


    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException(
                "Invalid stock quantity");

        Stock = quantity;
    }
}