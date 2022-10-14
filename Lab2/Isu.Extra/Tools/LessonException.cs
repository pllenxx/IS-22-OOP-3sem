namespace Isu.Extra.Tools;

public class LessonException : Exception
{
    public LessonException()
    {
    }

    public LessonException(string message)
        : base(message)
    {
    }

    public LessonException(string message, Exception inner)
        : base(message, inner)
    {
    }
}