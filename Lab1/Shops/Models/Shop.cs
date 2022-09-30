using Shops.Tools;

namespace Shops.Models;

public class Shop
{
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

    public Guid Id { get; }
    public decimal Money { get; set; }
    private string Name { get; }
    private string Address { get; }

    public IReadOnlyList<ShopItem> GetProducts() => _products;

    public IReadOnlyList<Product> GetProductsTypes() => _products.Select(product => product.Product).ToList().AsReadOnly();

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
        ShopItem item = new ShopItem(product, amount, price, this);
        if (_products.Contains(item))
        {
            item.Quantity += amount;
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
            return item.Quantity;
        throw new ShopException("There is no such product in this shop");
    }

    public ShopItem? ChangePrice(Product product, decimal newPrice)
    {
        if (product is null)
            throw new ProductException("No such product registered");
        ShopItem? item = _products.SingleOrDefault(p => p.Product == product);
        if (item is not null)
        {
            item.CurrentPrice = newPrice;
            return item;
        }

        throw new ProductException("There is no such product in this shop");
    }

    public void Purchase(Customer customer, Product product, int quantity)
    {
        ShopItem item = _products.Single(p => p.Product == product);
        if (item.Quantity < quantity)
            throw new ProductException("There is not enough product to buy");
        if (customer.Money < item.CurrentPrice * quantity)
            throw new CustomerException("Customer does not have enough money");
        customer.Money -= item.CurrentPrice * quantity;
        this.Money += item.CurrentPrice * quantity;
        item.Quantity -= quantity;
        if (item.Quantity == 0)
            _products.Remove(item);
        customer.AddProduct(item);
    }
}