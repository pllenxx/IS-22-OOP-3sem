using Banks.Tools;

namespace Banks;

public class CreditAccount : IBankAccount
{
    private decimal _money;
    private double _comission;
    public CreditAccount(Bank bank, Client client)
    {
        if (bank is null)
            throw new BanksException("Bank for credit account is null");
        if (client is null)
            throw new BanksException("Client for credit account is null");
        Id = Guid.NewGuid();
        Owner = client;
        BankBelonging = bank;
        _money = BankBelonging.GetSettings().CreditLimit;
        _comission = bank.GetSettings().CommissionForCreditUse;
    }

    public Guid Id { get; private set; }
    public Bank BankBelonging { get; private set; }
    public Client Owner { get; private set; }
    public decimal Balance => _money;

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
        if (_money < Constans.MinAmountOfMoney)
            _money = _money - moneyToTake - (_money * (decimal)_comission / 365);
        else
            _money -= moneyToTake;
    }

    public void FillUp(decimal moneyToTopOff)
    {
        if (moneyToTopOff <= Constans.MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        _money += moneyToTopOff;
    }

    public void Transfer(decimal moneyToTransfer, IBankAccount account)
    {
        if (moneyToTransfer <= Constans.MinAmountOfMoney)
            throw new BanksException("Amount must be greater than zero");
        if (account is null)
            throw new BanksException("Account to transfer money is null");
        if (_money < Constans.MinAmountOfMoney)
        {
            decimal sum = moneyToTransfer - (_money * (decimal)_comission / Constans.DaysInYear);
            _money -= sum;
            account.FillUp(moneyToTransfer);
        }
        else
        {
            _money -= moneyToTransfer;
            account.FillUp(moneyToTransfer);
        }
    }

    public void AddPercent()
    {
        if (_money < Constans.MinAmountOfMoney)
            _money -= _money * (decimal)BankBelonging.GetSettings().CommissionForCreditUse / Constans.MaxPercent;
    }
}