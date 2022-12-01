using Banks.Tools;

namespace Banks.Messages;

public class MessageForCreditCommissionUpdating : IMessage
{
    public string Message(IBankAccount account)
    {
        if (account is null)
            throw new BanksException("Account for message is null");
        string message = string.Format("Commission for usage for credit account {0} was updated", account.Id);
        return message;
    }
}