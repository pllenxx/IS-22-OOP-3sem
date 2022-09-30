using Shops.Tools;

namespace Shops.Models;

public class Customer
{
    private List<ShopItem> _customerProducts;
    public Customer(string name, decimal money)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CustomerException("Incorrect name input");
        Name = name;
        Id = Guid.NewGuid();
        Money = money;
        _customerProducts = new List<ShopItem>();
    }

    public Guid Id { get; }
    public decimal Money { get; set; }
    private string Name { get; }

    public void AddProduct(ShopItem item)
    {
        _customerProducts.Add(item);
    }

    public IReadOnlyList<Product> GetCustomerProducts() => _customerProducts.Select(product => product.Product).ToList().AsReadOnly();
}