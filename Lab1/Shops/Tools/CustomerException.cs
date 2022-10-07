namespace Shops.Tools;

public class CustomerException : Exception
{
    public CustomerException()
    {
    }

    public CustomerException(string message)
        : base(message)
    {
    }

    public CustomerException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
