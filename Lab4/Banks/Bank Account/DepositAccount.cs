using Banks.Tools;

namespace Banks;

public class DepositAccount : IBankAccount
{
    private const decimal MinAmountOfMoney = 0;

    private decimal _startedSum;
    private decimal _remainder;
    private DateTime _opening;
    private int _term;

    public DepositAccount(decimal money, Client client, Bank bank, DateTime date, int term)
    {
        Id = Guid.NewGuid();
        _startedSum = money;
        _remainder = money;
        Owner = client;
        BankBelonging = bank;
        _term = term;
        _opening = date;
    }

    public Guid Id { get; private set; }
    public Bank BankBelonging { get; private set; }
    public Client Owner { get; private set; }
    public decimal Balance => _remainder;

    public void Withdraw(decimal moneyToTake)
    {
        if (moneyToTake <= MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        if (moneyToTake > _remainder)
            throw new BanksException("Not enough money to take");
        if (_opening.AddMonths(_term) < DateTime.Now)
            throw new BanksException("Unable to perform operation");
        _remainder -= moneyToTake;
        BankBelonging.AddTransaction(new Transaction(this, null, moneyToTake));
    }

    public void FillUp(decimal moneyToTopOff)
    {
        if (moneyToTopOff <= MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        _remainder += moneyToTopOff;
        BankBelonging.AddTransaction(new Transaction(this, null, moneyToTopOff));
    }

    public void Transfer(decimal moneyToTransfer, IBankAccount account)
    {
        if (moneyToTransfer <= MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        if (account is null)
            throw new BanksException("Recipient account is null");
        if (_opening.AddMonths(_term) < DateTime.Now)
            throw new BanksException("Unable to perform operation");
        _remainder -= moneyToTransfer;
        account.FillUp(moneyToTransfer);
        BankBelonging.AddTransaction(new Transaction(this, account, moneyToTransfer));
    }

    public void AddPercent()
    {
        double percent = 0;
        if (_startedSum < BankBelonging.GetSettings().LowDepositSum)
        {
            percent = BankBelonging.GetSettings().LowDepositPercentage;
        }
        else if (_startedSum >= BankBelonging.GetSettings().LowDepositSum &&
                 _startedSum < BankBelonging.GetSettings().AverageDepositSum)
        {
            percent = BankBelonging.GetSettings().AverageDepositPercentage;
        }
        else
        {
            percent = BankBelonging.GetSettings().HighDepositPercentage;
        }

        decimal current = (_remainder / 100) * (decimal)(percent / 365);
        _remainder += current;
    }
}