using Shops.Models;
using Shops.Tools;

namespace Shops.Services;

public class ShopManager : IShopManager
{
    private const int MinAmountOfProductsInShop = 0;
    private const decimal MinCostForProduct = 0;
    private List<Shop> _listOfShops;
    private List<Customer> _listOfCustomers;
    private List<Product> _listOfProducts;
    private List<string> _listOfAddresses;

    public ShopManager()
    {
        _listOfShops = new List<Shop>();
        _listOfCustomers = new List<Customer>();
        _listOfProducts = new List<Product>();
        _listOfAddresses = new List<string>();
    }

    public Customer RegisterCustomer(string name, decimal money)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CustomerException("Incorrect customer name");
        Customer customer = new Customer(name, money);
        _listOfCustomers.Add(customer);
        return customer;
    }

    public Shop RegisterShop(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CustomerException("Incorrect shop name");
        if (string.IsNullOrWhiteSpace(address))
            throw new CustomerException("Incorrect shop address");
        if (_listOfAddresses.Contains(address))
            throw new ShopException("Shop with such address is already registered");
        _listOfAddresses.Add(address);
        Shop shop = new Shop(name, address);
        _listOfShops.Add(shop);
        return shop;
    }

    public Product RegisterProduct(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ProductException("Incorrect product name");
        Product product = new Product(name);
        _listOfProducts.Add(product);
        return product;
    }

    public Shop? FindShop(Shop shop)
    {
        return _listOfShops.SingleOrDefault(sh => sh.Id == shop.Id);
    }

    public Customer? FindCustomer(Customer customer)
    {
        return _listOfCustomers.SingleOrDefault(cus => cus.Id == customer.Id);
    }

    public Product? FindProduct(Product product)
    {
        return _listOfProducts.SingleOrDefault(pr => pr.Id == product.Id);
    }

    public void Shipment(Product product, Shop shop, decimal price, int quantity)
    {
        if (FindShop(shop) is null)
            throw new ShopException("This shop is not registered");
        if (FindProduct(product) is null)
            throw new ProductException("This product is not registered");
        if (price < MinCostForProduct)
            throw new ProductException("Price must be greater than zero");
        if (quantity < MinAmountOfProductsInShop)
            throw new ProductException("Product amount must be greater than zero");
        shop.AddProductToShop(product, price, quantity);
    }

    public void ChangeProductPrice(Shop shop, Product product, decimal newPrice)
    {
        if (FindShop(shop) is null)
            throw new ShopException("Shop is not found");
        if (FindProduct(product) is null)
            throw new ProductException("Product is not found");
        if (newPrice < MinCostForProduct)
            throw new ProductException("Incorrect price input");
        shop.ChangePrice(product, newPrice);
    }

    public void Purchase(Shop shop, Customer customer, Product product, int quantity)
    {
        if (FindShop(shop) is null)
            throw new ShopException("No such shop registered");
        if (FindCustomer(customer) is null)
            throw new CustomerException("No such customer registered");
        if (FindProduct(product) is null)
            throw new ProductException("No such product registered");
        if (quantity < MinAmountOfProductsInShop)
            throw new ProductException("Product amount must be greater than zero");
        shop.Purchase(customer, product, quantity);
    }

    public void Purchase(Shop shop, Customer customer, List<Product> products)
    {
        if (FindShop(shop) is null)
            throw new ShopException("Shop is not registered");
        if (products.Any(product => FindProduct(product) is null))
        {
            throw new ProductException("There is a product that was not registered");
        }

        if (products.Any(product => !shop.GetProductsTypes().Contains(product)))
        {
            throw new ShopException("This shop does not contain enough products");
        }

        foreach (var product in shop.GetProductsTypes())
        {
            shop.Purchase(customer, product, 1);
        }
    }

    public Shop? FindShopWithLowestPrices(Product product, int quantity)
    {
        if (FindProduct(product) is null)
            throw new ProductException("Product is not registered");
        if (quantity < MinAmountOfProductsInShop)
            throw new ProductException("Product amount must be greater than zero");
        decimal minPrice = decimal.MaxValue;
        Shop required = _listOfShops[0];
        foreach (var item in _listOfShops.Select(shop => shop.GetProductInShop(product)).Where(item => item.GetPrice() < minPrice && item.GetQuantity() >= quantity))
        {
            minPrice = item.GetPrice();
            required = item.CurrentShop;
        }

        return required ?? null;
    }

    public Shop? FindShopWithLowestPrices(List<Product> products)
    {
        if (products.Any(product => FindProduct(product) is null))
        {
            throw new ProductException("Product is not registered");
        }

        decimal sum = decimal.MaxValue, temp = 0;
        Shop required = _listOfShops[0];
        foreach (var shop in _listOfShops)
        {
            temp += products.Sum(product => shop.GetProductInShop(product).GetPrice());

            if (temp < sum)
            {
                sum = temp;
                required = shop;
            }
        }

        return required ?? null;
    }
}