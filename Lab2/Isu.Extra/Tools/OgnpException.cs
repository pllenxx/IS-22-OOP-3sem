namespace Isu.Extra.Tools;

public class OgnpException : Exception
{
    public OgnpException()
    {
    }

    public OgnpException(string message)
        : base(message)
    {
    }

    public OgnpException(string message, Exception inner)
        : base(message, inner)
    {
    }
}