namespace Business.Exceptions;

public class MessageProcessingSystemException : Exception
{
    public MessageProcessingSystemException()
    {
    }

    public MessageProcessingSystemException(string message)
        : base(message)
    {
    }

    public MessageProcessingSystemException(string message, Exception inner)
        : base(message, inner)
    {
    }
}