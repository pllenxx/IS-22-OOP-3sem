using Banks.Tools;

namespace Banks;

public class CreditAccount : IBankAccount
{
    private const decimal MinAmountOfMoney = 0;

    private decimal _money;
    private double _comission;
    public CreditAccount(Bank bank, Client client)
    {
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
        if (moneyToTake <= MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        if (_money < 0)
            _money = _money - moneyToTake - (_money * (decimal)_comission / 365);
        else
            _money -= moneyToTake;
    }

    public void FillUp(decimal moneyToTopOff)
    {
        if (moneyToTopOff <= MinAmountOfMoney)
            throw new BanksException("Sum must be greater than 0");
        _money += moneyToTopOff;
    }

    public void Transfer(decimal moneyToTransfer, IBankAccount account)
    {
        if (moneyToTransfer <= MinAmountOfMoney)
            throw new BanksException("Amount must be greater than zero");
        if (account is null)
            throw new BanksException("Account to transfer money is null");
        if (_money < 0)
        {
            decimal sum = moneyToTransfer - (_money * (decimal)_comission / 365);
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
        if (_money < 0)
            _money -= _money * (decimal)BankBelonging.GetSettings().CommissionForCreditUse / 100;
    }
}