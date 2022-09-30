using Shops.Models;
using Shops.Services;
using Shops.Tools;
using Xunit;

namespace Shops.Test;

public class ShopsTest
{
    private ShopManager _shopManager = new ShopManager();

    [Fact]
    public void AddProductsToShop_ShopContainsProducts()
    {
        var shop = _shopManager.RegisterShop("Ashan", "Kronverkskiy 49");
        var product1 = _shopManager.RegisterProduct("Apple");
        var product2 = _shopManager.RegisterProduct("Chocolate");
        var product3 = _shopManager.RegisterProduct("Water");
        _shopManager.Shipment(product1, shop, 32, 4);
        _shopManager.Shipment(product2, shop, 20, 100);
        Assert.Contains(product1, shop.GetProductsTypes());
        Assert.True(shop.GetProducts()[0].Quantity == 4);
        Assert.Contains(product2, shop.GetProductsTypes());
        Assert.True(shop.GetProducts()[1].Quantity == 100);
        Assert.DoesNotContain(product3, shop.GetProductsTypes());
    }

    [Fact]
    public void SetPrice_ChangePrice()
    {
        var shop = _shopManager.RegisterShop("Lenta", "Lomonosovskaya 9");
        var product = _shopManager.RegisterProduct("Kolbasa");
        _shopManager.Shipment(product, shop, 10, 3);
        Assert.True(shop.GetProducts()[0].CurrentPrice == 10);
        _shopManager.ChangeProductPrice(shop, product, 15);
        Assert.True(shop.GetProducts()[0].CurrentPrice == 15);
    }

    [Fact]
    public void ShopWithLowestPrices()
    {
        var shop1 = _shopManager.RegisterShop("Magnit", "Chaikovskogo 228");
        var shop2 = _shopManager.RegisterShop("Another Magnit", "Chaikovskogo 229");
        var product1 = _shopManager.RegisterProduct("Sol'");
        var product2 = _shopManager.RegisterProduct("Saxar");
        _shopManager.Shipment(product1, shop1, 1000000, 4);
        _shopManager.Shipment(product1, shop2, 2, 100);
        Assert.Equal(_shopManager.FindShopWithLowestPrices(product1, 2), shop2);
        Assert.Throws<ShopException>(() => _shopManager.FindShopWithLowestPrices(product2, 10000));
    }

    [Fact]
    public void BuyProducts()
    {
        var shop = _shopManager.RegisterShop("Dicksi", "Birzhevaya linia 14");
        var product = _shopManager.RegisterProduct("Toys");
        var customer1 = _shopManager.RegisterCustomer("Sveta Kpop", 50);
        _shopManager.Shipment(product, shop, 7, 100);
        _shopManager.Purchase(shop, customer1, product, 3);
        Assert.Contains(product, customer1.GetCustomerProducts());
        Assert.True(customer1.Money == 50 - 21);
        Assert.True(shop.GetQuantityOfProduct(product) == 97);
        Assert.True(shop.Money == 21);
        var customer2 = _shopManager.RegisterCustomer("Btbks", 2);
        Assert.Throws<CustomerException>(() => _shopManager.Purchase(shop, customer2, product, 1));
    }
}