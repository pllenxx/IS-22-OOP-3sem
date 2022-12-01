using Banks.Tools;
namespace Banks;

public class DepositAccountCreator : BankAccountCreator
{
    private readonly decimal _money;
    private readonly DateTime _opening;
    private readonly int _term;

    public DepositAccountCreator(Bank bank, Client client, decimal amountOfMoney, DateTime date, int time)
        : base(bank, client)
    {
        if (bank is null)
            throw new BanksException("Unable to create account with null bank");
        if (client is null)
            throw new BanksException("Unable to create account with null client");
        if (amountOfMoney < Constans.MinAmountOfMoney)
            throw new BanksException("Unable to create account with negative amount of money");
        if (date == DateTime.MinValue)
            throw new BanksException("Something is wrong with the date");
        if (time < Constans.MinTerm)
            throw new BanksException("Unable to create account with such short term");
        _money = amountOfMoney;
        _opening = date;
        _term = time;
    }

    public override IBankAccount CreateAccount()
    {
        return new DepositAccount(_money, Client, BankBelonging, _opening, _term);
    }
}