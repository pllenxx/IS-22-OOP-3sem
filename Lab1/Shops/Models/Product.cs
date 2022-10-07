using Shops.Tools;

namespace Shops.Models;

public class Product
{
    public Product(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ProductException("Invalid name input");
        Name = name;
        Id = name.GetHashCode();
    }

    public int Id { get; }
    private string Name { get; }
}