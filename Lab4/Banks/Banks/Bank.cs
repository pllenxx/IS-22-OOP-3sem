using Banks.Messages;
using Banks.Observer;
using Banks.Tools;

namespace Banks;

public class Bank : IObservable
{
    private const double MinPercent = 0;

    private Guid _id;
    private string _name;
    private decimal _bankMoney;
    private BankSettings _settings;
    private List<Client> _clients;
    private List<IBankAccount> _accounts;
    private List<Transaction> _transactions;
    private List<IObserver> _observers;

    public Bank(string name, BankSettings settings)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Bank's name is empty");
        if (settings is null)
            throw new BanksException("Settings is null");
        _id = Guid.NewGuid();
        _name = name;
        _bankMoney = settings.AuthorizedCapital;
        _settings = settings;
        _clients = new List<Client>();
        _accounts = new List<IBankAccount>();
        _transactions = new List<Transaction>();
        _observers = new List<IObserver>();
    }

    public string Name => _name;
    public decimal BankMoney => _bankMoney;
    public IReadOnlyList<Client> Clients => _clients.AsReadOnly();
    public IReadOnlyList<IBankAccount> Accounts => _accounts.AsReadOnly();
    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();
    public BankSettings GetSettings() => _settings;

    public void AddClient(Client client)
    {
        if (client is null)
            throw new BanksException("Unable to add null client");
        _clients.Add(client);
    }

    public IBankAccount CreateDepositAccount(Client client, decimal moneyToPut, DateTime start, int period)
    {
        if (client is null)
            throw new BanksException("Unable to create an account as client is null");
        if (moneyToPut < 0)
            throw new BanksException("Money to create an account is null");
        if (period < 0)
            throw new BanksException("Period of account is negative");
        BankAccountCreator creator = new DepositAccountCreator(this, client, moneyToPut, start, period);
        IBankAccount account = creator.CreateAccount();
        _accounts.Add(account);
        if (_clients.Contains(client))
        {
            return account;
        }

        _bankMoney += moneyToPut;

        _clients.Add(client);
        return account;
    }

    public IBankAccount CreateDebitAccount(Client client, decimal moneyToPut)
    {
        if (client is null)
            throw new BanksException("Unable to create an account as client is null");
        if (moneyToPut < 0)
            throw new BanksException("Money to create an account is null");
        BankAccountCreator creator = new DebitAccountCreator(this, client, moneyToPut);
        IBankAccount account = creator.CreateAccount();
        _accounts.Add(account);
        _bankMoney += moneyToPut;
        if (_clients.Contains(client))
        {
            return account;
        }

        _clients.Add(client);
        return account;
    }

    public IBankAccount CreateCreditAccount(Client client)
    {
        if (client is null)
            throw new BanksException("Unable to create an account as client is null");
        BankAccountCreator creator = new CreditAccountCreator(this, client);
        IBankAccount account = creator.CreateAccount();
        _accounts.Add(account);
        if (_clients.Contains(client))
        {
            return account;
        }

        _clients.Add(client);
        return account;
    }

    public void AddTransaction(Transaction transaction)
    {
        if (transaction is null)
            throw new BanksException("Nothing to add");
        _transactions.Add(transaction);
        var centralBank = CentralBank.GetInstance();
        centralBank.CheckTransaction(transaction);
    }

    public void ChangeConditionsForDebit(double newPercent)
    {
        _settings.UpdateDebitPercentage(newPercent);
        NotifyObservers(new MessageForUpdatingPercents());
    }

    public void ChangeLowPercentForDeposit(double newPercent)
    {
        if (newPercent < 0)
            throw new BanksException("Percent cannot be negative");
        _settings.UpdateLowDepositPercentage(newPercent);
        NotifyObservers(new MessageForUpdatingPercents());
    }

    public void ChangeAveragePercentForDeposit(double newPercent)
    {
        if (newPercent < 0)
            throw new BanksException("Percent cannot be negative");
        _settings.UpdateAverageDepositPercentage(newPercent);
        NotifyObservers(new MessageForUpdatingPercents());
    }

    public void ChangeHighPercentForDeposit(double newPercent)
    {
        if (newPercent < 0)
            throw new BanksException("Percent cannot be negative");
        _settings.UpdateHighDepositPercentage(newPercent);
        NotifyObservers(new MessageForUpdatingPercents());
    }

    public void ChangeCreditLimit(decimal newLimit)
    {
        if (newLimit < 0)
            throw new BanksException("New money limit must be higher than 0");
        _settings.UpdateCreditLimit(newLimit);
        NotifyObservers(new MessageForCreditLimitChange());
    }

    public void ChangeCommissionForCreditUse(double newCommission)
    {
        if (newCommission < 0)
            throw new BanksException("Commission percent cannot be negative");
        _settings.UpdateCommissionForCreditUse(newCommission);
        NotifyObservers(new MessageForCreditCommissionUpdating());
    }

    public void UpdateAccounts(int days)
    {
        for (int i = days; days >= 0; days--)
        {
            foreach (var account in _accounts)
            {
                account.AddPercent();
            }
        }
    }

    public void AddObserver(IObserver account)
    {
        _observers.Add(account);
    }

    public void RemoveObserver(IObserver account)
    {
        _observers.Remove(account);
    }

    public void NotifyObservers(IMessage message)
    {
        foreach (var observer in _observers)
        {
            observer.Update(message);
        }
    }
}