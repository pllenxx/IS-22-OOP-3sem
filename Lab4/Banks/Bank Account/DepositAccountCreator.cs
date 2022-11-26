namespace Banks;

public class DepositAccountCreator : BankAccountCreator
{
    private readonly decimal _money;
    private readonly DateTime _opening;
    private readonly int _term;

    public DepositAccountCreator(Bank bank, Client client, decimal amountOfMoney, DateTime date, int time)
        : base(bank, client)
    {
        _money = amountOfMoney;
        _opening = date;
        _term = time;
    }

    public override IBankAccount CreateAccount()
    {
        return new DepositAccount(_money, Client, BankBelonging, _opening, _term);
    }
}