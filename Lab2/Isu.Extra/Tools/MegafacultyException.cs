namespace Isu.Extra.Tools;

public class MegafacultyException : Exception
{
    public MegafacultyException()
    {
    }

    public MegafacultyException(string message)
        : base(message)
    {
    }

    public MegafacultyException(string message, Exception inner)
        : base(message, inner)
    {
    }
}