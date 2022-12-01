using Banks.Tools;

namespace Banks;

public class DepositAccount : IBankAccount
{
    private decimal _startedSum;
    private decimal _remainder;
    private DateTime _opening;
    private int _term;

    public DepositAccount(decimal money, Client client, Bank bank, DateTime date, int term)
    {
        if (money < Constans.MinAmountOfMoney)
            throw new BanksException("Amount of money is negative");
        if (client is null)
            throw new BanksException("Client for deposit account is null");
        if (bank is null)
            throw new BanksException("Bank for deposit account is null");
        if (date == DateTime.MinValue)
            throw new BanksException("Something is wrong with the date");
        if (term < Constans.MinTerm)
            throw new BanksException("Deposit term is too short");
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

    public bool IsTransactionPossible(Client owner, Client? recipient)
    {
        if (owner is null)
            throw new BanksException("Account owner is null");
        if (recipient is null && owner.CheckDoubtfulness())
        {
            return false;
        }

        return recipient is not null && !recipient.CheckDoubtfulness() && !owner.CheckDoubtfulness();
    }

    public void Withdraw(decimal moneyToTake)
    {
        if (moneyToTake <= Constans.MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        if (moneyToTake > _remainder)
            throw new BanksException("Not enough money to take");
        if (_opening.AddMonths(_term) < DateTime.Now)
            throw new BanksException("Unable to perform operation");
        _remainder -= moneyToTake;
    }

    public void FillUp(decimal moneyToTopOff)
    {
        if (moneyToTopOff <= Constans.MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        _remainder += moneyToTopOff;
    }

    public void Transfer(decimal moneyToTransfer, IBankAccount account)
    {
        if (moneyToTransfer <= Constans.MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        if (account is null)
            throw new BanksException("Recipient account is null");
        if (_opening.AddMonths(_term) < DateTime.Now)
            throw new BanksException("Unable to perform operation");
        _remainder -= moneyToTransfer;
        account.FillUp(moneyToTransfer);
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

        decimal current = (_remainder / Constans.MaxPercent) * (decimal)(percent / Constans.DaysInYear);
        _remainder += current;
    }
}