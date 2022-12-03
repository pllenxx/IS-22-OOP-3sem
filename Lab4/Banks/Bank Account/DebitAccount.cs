using Banks.Tools;
using Microsoft.VisualBasic;

namespace Banks;

public class DebitAccount : IBankAccount
{
    private decimal _money;
    public DebitAccount(decimal money, Bank bank, Client client)
    {
        if (bank is null)
            throw new BanksException("Bank for debit account is null");
        if (client is null)
            throw new BanksException("Client for debit account is null");
        if (money < Constans.MinAmountOfMoney)
            throw new BanksException("Unable to create account with negative amount of money");
        Id = Guid.NewGuid();
        _money = money;
        BankBelonging = bank;
        Owner = client;
    }

    public Guid Id { get; private set; }
    public Bank BankBelonging { get; private set; }
    public Client Owner { get; private set; }
    public decimal Balance => _money;

    public bool IsTransactionPossible(Client owner, Client? recipient)
    {
        if (owner is null)
            throw new BanksException("Account owner is null");
        if (recipient is null)
        {
            return owner.CheckDoubtfulness();
        }

        return owner.CheckDoubtfulness() && recipient.CheckDoubtfulness();
    }

    public void Withdraw(decimal moneyToTake)
    {
        if (moneyToTake <= Constans.MinAmountOfMoney)
            throw new BanksException("Amount must be greater than zero");
        if (moneyToTake > _money)
            throw new BanksException("Too much money to take");
        _money -= moneyToTake;
    }

    public void FillUp(decimal moneyToTopOff)
    {
        if (moneyToTopOff <= Constans.MinAmountOfMoney)
            throw new BanksException("Amount must be greater than zero");
        _money += moneyToTopOff;
    }

    public void Transfer(decimal moneyToTransfer, IBankAccount account)
    {
        if (moneyToTransfer <= Constans.MinAmountOfMoney)
            throw new BanksException("Amount must be greater than zero");
        if (account is null)
            throw new BanksException("Account to transfer money is null");
        if (moneyToTransfer > _money)
            throw new BanksException("Too much money to take");
        _money -= moneyToTransfer;
        account.FillUp(moneyToTransfer);
    }

    public void AddPercent()
    {
        _money += (_money / Constans.MaxPercent) *
                  (decimal)(BankBelonging.GetSettings().DebitPercentage / Constans.DaysInYear);
    }
}