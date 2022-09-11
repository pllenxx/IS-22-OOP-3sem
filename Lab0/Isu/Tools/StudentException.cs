namespace Isu.Tools;

public class StudentException : Exception
{
    public StudentException()
    {
    }

    public StudentException(string message)
        : base(message)
    {
    }

    public StudentException(string message, Exception inner)
        : base(message, inner)
    {
    }
}