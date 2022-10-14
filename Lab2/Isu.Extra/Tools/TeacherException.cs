namespace Isu.Extra.Tools;

public class TeacherException : Exception
{
    public TeacherException()
    {
    }

    public TeacherException(string message)
        : base(message)
    {
    }

    public TeacherException(string message, Exception inner)
        : base(message, inner)
    {
    }
}