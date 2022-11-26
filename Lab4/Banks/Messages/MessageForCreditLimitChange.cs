namespace Banks.Messages;

public class MessageForCreditLimitChange : IMessage
{
    public string Message(IBankAccount account)
    {
        string message = string.Format("Credit limit for account {0} was changed", account.Id);
        return message;
    }
}