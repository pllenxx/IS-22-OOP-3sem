using Banks.Tools;

namespace Banks;

public class DebitAccount : IBankAccount
{
    private const decimal MinAmountOfMoney = 0;
    private decimal _money;
    public DebitAccount(decimal money, Bank bank, Client client)
    {
        Id = Guid.NewGuid();
        _money = money;
        BankBelonging = bank;
        Owner = client;
    }

    public Guid Id { get; private set; }
    public Bank BankBelonging { get; private set; }
    public Client Owner { get; private set; }
    public decimal Balance => _money;

    public void Withdraw(decimal moneyToTake)
    {
        if (moneyToTake <= MinAmountOfMoney)
            throw new BanksException("Amount must be greater than zero");
        if (moneyToTake > _money)
            throw new BanksException("Too much money to take");
        _money -= moneyToTake;
    }

    public void FillUp(decimal moneyToTopOff)
    {
        if (moneyToTopOff <= MinAmountOfMoney)
            throw new BanksException("Amount must be greater than zero");
        _money += moneyToTopOff;
    }

    public void Transfer(decimal moneyToTransfer, IBankAccount account)
    {
        if (moneyToTransfer <= MinAmountOfMoney)
            throw new BanksException("Amount must be greater than zero");
        if (account is null)
            throw new BanksException("Account to transfer money is null");
        if (moneyToTransfer > _money)
            throw new BanksException("Too much money to take");
        _money -= moneyToTransfer;
        account.FillUp(moneyToTransfer);
        BankBelonging.AddTransaction(new Transaction(this, account, moneyToTransfer));
    }

    public void AddPercent()
    {
        _money += _money / 100 * (decimal)(BankBelonging.GetSettings().DebitPercentage / 365);
    }
}