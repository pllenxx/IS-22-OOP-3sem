using Shops.Tools;

namespace Shops.Models;

public class Shop
{
    private const int MinAmountOfProductsInShop = 0;
    private const decimal MinCostForProduct = 0;
    private List<ShopItem> _products;
    public Shop(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ShopException("Incorrect name input");
        if (string.IsNullOrWhiteSpace(address))
            throw new ShopException("Incorrect address input");
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Money = 0;
        _products = new List<ShopItem>();
    }

    public IReadOnlyList<ShopItem> Products => _products;
    public Guid Id { get; }
    private decimal Money { get; set; }
    private string Name { get; }
    private string Address { get; }

    public IReadOnlyList<Product> GetProductsTypes() => _products.Select(product => product.Product).ToList().AsReadOnly();

    public decimal GetShopMoney()
    {
        return Money;
    }

    public ShopItem GetProductInShop(Product product)
    {
        ShopItem? item = _products.SingleOrDefault(p => p.Product == product);
        if (item is not null)
            return item;
        throw new ShopException("This product was not found");
    }

    public void AddProductToShop(Product product, decimal price, int amount)
    {
        if (product is null)
            throw new ProductException("No such product registered");
        if (amount < MinAmountOfProductsInShop)
            throw new ShopException("Amount must be greater than 0");
        ShopItem item = new ShopItem(product, amount, price, this);
        if (_products.Contains(item))
        {
            item.SetQuantity(item.GetQuantity() + amount);
        }
        else
        {
            _products.Add(item);
        }
    }

    public int GetQuantityOfProduct(Product product)
    {
        if (product is null)
            throw new ProductException("No such product registered");
        ShopItem? item = _products.SingleOrDefault(p => p.Product == product);
        if (item is not null)
            return item.GetQuantity();
        throw new ShopException("There is no such product in this shop");
    }

    public ShopItem? ChangePrice(Product product, decimal newPrice)
    {
        if (product is null)
            throw new ProductException("No such product registered");
        if (newPrice < MinCostForProduct)
            throw new ShopException("Price must be greater than 0");
        ShopItem? item = _products.SingleOrDefault(p => p.Product == product);
        if (item is not null)
        {
            item.SetPrice(newPrice);
            return item;
        }

        throw new ProductException("There is no such product in this shop");
    }

    public void Purchase(Customer customer, Product product, int quantity)
    {
        ShopItem item = _products.Single(p => p.Product == product);
        if (item.GetQuantity() < quantity)
            throw new ProductException("There is not enough product to buy");
        if (customer.GetMoney() < item.GetPrice() * quantity)
            throw new CustomerException("Customer does not have enough money");
        customer.SetMoney(customer.GetMoney() - (item.GetPrice() * quantity));
        Money += item.GetPrice() * quantity;
        item.SetQuantity(item.GetQuantity() - quantity);
        if (item.GetQuantity() == MinAmountOfProductsInShop)
            _products.Remove(item);
        customer.AddProduct(item);
    }
}