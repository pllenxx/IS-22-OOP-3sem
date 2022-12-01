using Banks.Tools;
namespace Banks;

public class CreditAccountCreator : BankAccountCreator
{
    public CreditAccountCreator(Bank bank, Client client)
        : base(bank, client)
    {
        if (bank is null)
            throw new BanksException("Unable to create account with null bank");
        if (client is null)
            throw new BanksException("Unable to create account with null client");
    }

    public override IBankAccount CreateAccount()
    {
        return new CreditAccount(BankBelonging, Client);
    }
}