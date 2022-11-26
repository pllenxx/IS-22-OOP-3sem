namespace Banks;

public class CreditAccountCreator : BankAccountCreator
{
    public CreditAccountCreator(Bank bank, Client client)
        : base(bank, client)
    {
    }

    public override IBankAccount CreateAccount()
    {
        return new CreditAccount(BankBelonging, Client);
    }
}