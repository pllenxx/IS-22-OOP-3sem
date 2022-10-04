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
        Assert.True(shop.Products[0].GetQuantity() == 4);
        Assert.Contains(product2, shop.GetProductsTypes());
        Assert.True(shop.Products[1].GetQuantity() == 100);
        Assert.DoesNotContain(product3, shop.GetProductsTypes());
    }

    [Fact]
    public void SetPrice_ChangePrice()
    {
        var shop = _shopManager.RegisterShop("Lenta", "Lomonosovskaya 9");
        var product = _shopManager.RegisterProduct("Kolbasa");
        _shopManager.Shipment(product, shop, 10, 3);
        Assert.True(shop.Products[0].GetPrice() == 10);
        _shopManager.ChangeProductPrice(shop, product, 15);
        Assert.True(shop.Products[0].GetPrice() == 15);
    }

    [Fact]
    public void ShopWithLowestPrices()
    {
        var shop1 = _shopManager.RegisterShop("Magnit", "Chaikovskogo 228");
        var shop2 = _shopManager.RegisterShop("Another Magnit", "Chaikovskogo 229");
        var product1 = _shopManager.RegisterProduct("Sol'");
        var product2 = _shopManager.RegisterProduct("Saxar");
        var product3 = _shopManager.RegisterProduct("Tomato");
        var product4 = _shopManager.RegisterProduct("Cucucmber");
        _shopManager.Shipment(product1, shop1, 1000000, 4);
        _shopManager.Shipment(product1, shop2, 2, 100);
        _shopManager.Shipment(product3, shop1, 25, 7);
        _shopManager.Shipment(product4, shop1, 20, 6);
        _shopManager.Shipment(product3, shop2, 100, 7);
        _shopManager.Shipment(product4, shop2, 110, 6);
        var products = new List<Product>() { product3, product4 };
        Assert.Equal(_shopManager.FindShopWithLowestPrices(products), shop1);
        Assert.Equal(_shopManager.FindShopWithLowestPrices(product1, 2), shop2);
        Assert.Throws<ShopException>(() => _shopManager.FindShopWithLowestPrices(product2, 10000));
    }

    [Fact]
    public void BuyProducts()
    {
        var shop = _shopManager.RegisterShop("Dicksi", "Birzhevaya linia 14");
        var product1 = _shopManager.RegisterProduct("Toys");
        var customer1 = _shopManager.RegisterCustomer("Sveta Kpop", 50);
        Assert.Throws<ShopException>(() => _shopManager.RegisterShop("Diksi Imposter", "Birzhevaya linia 14"));
        _shopManager.Shipment(product1, shop, 7, 100);
        _shopManager.Purchase(shop, customer1, product1, 3);
        var product2 = _shopManager.RegisterProduct("Candy");
        Assert.Contains(product1, customer1.GetCustomerProducts());
        Assert.True(customer1.GetMoney() == 50 - 21);
        Assert.True(shop.GetQuantityOfProduct(product1) == 97);
        Assert.True(shop.GetShopMoney() == 21);
        var customer2 = _shopManager.RegisterCustomer("Btbks", 2);
        Assert.Throws<CustomerException>(() => _shopManager.Purchase(shop, customer2, product1, 1));
        _shopManager.Shipment(product2, shop, 10, 34);
        var products = new List<Product>() { product1, product2 };
        var customer3 = _shopManager.RegisterCustomer("Andrik", 100);
        _shopManager.Purchase(shop, customer3, products);
        Assert.Contains(product1, customer3.GetCustomerProducts());
        Assert.True(customer3.GetMoney() == 100 - 7 - 10);
        Assert.True(shop.GetQuantityOfProduct(product1) == 96);
        Assert.True(shop.GetQuantityOfProduct(product2) == 33);
        Assert.True(shop.GetShopMoney() == 38);
    }
}