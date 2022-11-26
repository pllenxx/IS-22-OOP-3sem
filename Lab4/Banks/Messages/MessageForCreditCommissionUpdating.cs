namespace Banks.Messages;

public class MessageForCreditCommissionUpdating : IMessage
{
    public string Message(IBankAccount account)
    {
        string message = string.Format("Commission for usage for credit account {0} was updated", account.Id);
        return message;
    }
}