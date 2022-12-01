namespace Banks.Messages;

public interface IMessage
{
    string Message(IBankAccount account);
}