using System.Net;
using Shops.Models;
using Shops.Tools;

namespace Shops.Models;

public class ShopItem
{
    private const int MinAmountOfProductsInShop = 0;
    private const decimal MinCostForProduct = 0;
    public ShopItem(Product product, int quantity, decimal price, Shop shop)
    {
        if (quantity < MinAmountOfProductsInShop)
            throw new ProductException("Quantity cannot be less than 0");
        if (price < MinCostForProduct)
            throw new ProductException("Price cannot be less than 0");
        Product = product;
        Quantity = quantity;
        CurrentPrice = price;
        CurrentShop = shop;
    }

    public Product Product { get; }
    public Shop CurrentShop { get; }
    private decimal CurrentPrice { get; set; }
    private int Quantity { get; set; }

    public int GetQuantity()
    {
        return Quantity;
    }

    public void SetQuantity(int amount)
    {
        Quantity = amount;
    }

    public decimal GetPrice()
    {
        return CurrentPrice;
    }

    public void SetPrice(decimal price)
    {
        CurrentPrice = price;
    }
}