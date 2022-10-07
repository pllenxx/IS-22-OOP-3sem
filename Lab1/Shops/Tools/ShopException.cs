namespace Shops.Tools;

public class ShopException : Exception
{
    public ShopException()
    {
    }

    public ShopException(string message)
        : base(message)
    {
    }

    public ShopException(string message, Exception inner)
        : base(message, inner)
    {
    }
}