using Banks.Tools;
namespace Banks.Messages;

public class MessageForUpdatingPercents : IMessage
{
    public string Message(IBankAccount account)
    {
        if (account is null)
            throw new BanksException("Account for message is null");
        string message = string.Format("Percentage of the balance for the account {0} was updated", account.Id);
        return message;
    }
}