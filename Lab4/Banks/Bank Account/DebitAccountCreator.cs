using Banks.Tools;
namespace Banks;

public class DebitAccountCreator : BankAccountCreator
{
    private readonly decimal _money;

    public DebitAccountCreator(Bank bank, Client client, decimal amountOfMoney)
        : base(bank, client)
    {
        if (bank is null)
            throw new BanksException("Unable to create account with null bank");
        if (client is null)
            throw new BanksException("Unable to create account with null client");
        if (amountOfMoney < Constans.MinAmountOfMoney)
            throw new BanksException("Unable to create account with negative amount of money");
        _money = amountOfMoney;
    }

    public override IBankAccount CreateAccount()
    {
        return new DebitAccount(_money, BankBelonging, Client);
    }
}