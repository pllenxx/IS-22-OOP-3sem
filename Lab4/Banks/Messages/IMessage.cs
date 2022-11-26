namespace Banks.Messages;

public interface IMessage
{
    public string Message(IBankAccount account);
}