using System.Net;
using Shops.Models;
using Shops.Tools;

namespace Shops.Models;

public class ShopItem
{
    public ShopItem(Product product, int quantity, decimal price, Shop shop)
    {
        if (quantity < 0)
            throw new ProductException("Quantity cannot be less than 0");
        if (price < 0)
            throw new ProductException("Price cannot be less than 0");
        Product = product;
        Quantity = quantity;
        CurrentPrice = price;
        CurrentShop = shop;
    }

    public decimal CurrentPrice { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; }
    public Shop CurrentShop { get; }
}