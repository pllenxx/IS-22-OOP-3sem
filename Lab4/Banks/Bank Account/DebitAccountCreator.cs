namespace Banks;

public class DebitAccountCreator : BankAccountCreator
{
    private readonly decimal _money;

    public DebitAccountCreator(Bank bank, Client client, decimal amountOfMoney)
        : base(bank, client)
    {
        _money = amountOfMoney;
    }

    public override IBankAccount CreateAccount()
    {
        return new DebitAccount(_money, BankBelonging, Client);
    }
}