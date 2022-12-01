using Banks.Tools;
namespace Banks.Messages;

public class MessageForCreditLimitChange : IMessage
{
    public string Message(IBankAccount account)
    {
        if (account is null)
            throw new BanksException("Account for message is null");
        string message = string.Format("Credit limit for account {0} was changed", account.Id);
        return message;
    }
}