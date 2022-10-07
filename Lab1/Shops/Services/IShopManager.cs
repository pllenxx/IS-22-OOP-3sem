using Shops.Models;

namespace Shops.Services;

public interface IShopManager
{
    Customer RegisterCustomer(string name, decimal money);

    Shop RegisterShop(string name, string address);

    Product RegisterProduct(string name);

    Shop? FindShop(Shop shop);

    Customer? FindCustomer(Customer customer);

    Product? FindProduct(Product product);

    void Shipment(Product product, Shop shop, decimal price, int quantity);

    void ChangeProductPrice(Shop shop, Product product, decimal newPrice);

    void Purchase(Shop shop, Customer customer, Product product, int quantity);

    void Purchase(Shop shop, Customer customer, List<Product> products);

    Shop? FindShopWithLowestPrices(Product product, int quantity);

    Shop? FindShopWithLowestPrices(List<Product> products);
}