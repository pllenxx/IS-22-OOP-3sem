namespace Isu.Extra.Tools;

public class TimeException : Exception
{
    public TimeException()
    {
    }

    public TimeException(string message)
        : base(message)
    {
    }

    public TimeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}