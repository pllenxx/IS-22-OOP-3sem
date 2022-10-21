namespace Isu.Extra.Tools;

public class OgnpSubjectException : Exception
{
    public OgnpSubjectException()
    {
    }

    public OgnpSubjectException(string message)
        : base(message)
    {
    }

    public OgnpSubjectException(string message, Exception inner)
        : base(message, inner)
    {
    }
}