using Shops.Tools;

namespace Shops.Models;

public class Customer
{
    private const decimal MinAmountOfMoney = 0;
    private List<ShopItem> _customerProducts;
    public Customer(string name, decimal money)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CustomerException("Incorrect name input");
        if (money < MinAmountOfMoney)
            throw new CustomerException("Customer cannot have amount of money less than 0");
        Name = name;
        Id = Guid.NewGuid();
        Money = money;
        _customerProducts = new List<ShopItem>();
    }

    public Guid Id { get; }
    private decimal Money { get; set; }
    private string Name { get; }

    public decimal GetMoney()
    {
        return Money;
    }

    public void SetMoney(decimal money)
    {
        if (money < MinAmountOfMoney)
            throw new CustomerException("Customer cannot have amount of money less tha 0");
        Money = money;
    }

    public void AddProduct(ShopItem item)
    {
        if (item is null)
            throw new ProductException("Product was not found");
        _customerProducts.Add(item);
    }

    public IReadOnlyList<Product> GetCustomerProducts() => _customerProducts.Select(product => product.Product).ToList().AsReadOnly();
}