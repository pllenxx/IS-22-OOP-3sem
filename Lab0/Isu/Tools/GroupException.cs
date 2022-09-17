using System;

namespace Isu.Tools;

public class GroupException : Exception
{
    public GroupException()
    {
    }

    public GroupException(string message)
        : base(message)
    {
    }

    public GroupException(string message, Exception inner)
        : base(message, inner)
    {
    }
}