using System.Runtime.InteropServices.ComTypes;
using Banks.Tools;

namespace Banks;

public class CentralBank
{
    private const decimal MinCapital = 1000000000;
    private static CentralBank? _centralBank;
    private List<Bank> _banks;
    private List<Transaction> _transactions;
    private DateTime _actualDate;

    private CentralBank()
    {
        _banks = new List<Bank>();
        _transactions = new List<Transaction>();
        _actualDate = DateTime.Now;
    }

    public IReadOnlyList<Bank> Banks => _banks.AsReadOnly();
    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();
    public static CentralBank GetInstance()
    {
        return _centralBank ?? (_centralBank = new CentralBank());
    }

    public Bank RegisterBank(string bankName, BankSettings settings)
    {
        if (string.IsNullOrWhiteSpace(bankName))
            throw new BanksException("Bank's name is incorrect");
        if (settings.AuthorizedCapital < MinCapital)
            throw new BanksException("Not enough money to create a bank");
        var bank = new Bank(bankName, settings);
        _banks.Add(bank);
        return bank;
    }

    public IBankAccount FindAccountById(Guid id)
    {
        foreach (var account in from bank in _banks from account in bank.Accounts where account.Id == id select account)
        {
            return account;
        }

        throw new BanksException("Account not found");
    }

    public void AddMoneyToAccount(IBankAccount account, decimal moneyToAdd)
    {
        if (account is null)
            throw new BanksException("Account to add money is null");
        if (moneyToAdd < Constans.MinAmountOfMoney)
            throw new BanksException("Input larger sum");
        if (account.IsTransactionPossible(account.Owner, null))
        {
            Transaction transaction = new Transaction(Guid.NewGuid(), account, null, moneyToAdd);
            account.FillUp(moneyToAdd);
            _transactions.Add(transaction);
            account.BankBelonging.AddTransaction(transaction);
        }
        else
        {
            Transaction transaction = new Transaction(Guid.NewGuid(), account, null, moneyToAdd);
            transaction.Cancel();
            _transactions.Add(transaction);
            account.BankBelonging.AddTransaction(transaction);
        }
    }

    public void ReduceMoneyFromAccount(IBankAccount account, decimal moneyToTake)
    {
        if (account is null)
            throw new BanksException("Account to add money is null");
        if (moneyToTake < Constans.MinAmountOfMoney)
            throw new BanksException("Input larger sum");
        if (account.IsTransactionPossible(account.Owner, null))
        {
            Transaction transaction = new Transaction(Guid.NewGuid(), account, null, moneyToTake);
            account.Withdraw(moneyToTake);
            _transactions.Add(transaction);
            account.BankBelonging.AddTransaction(transaction);
        }
        else
        {
            Transaction transaction = new Transaction(Guid.NewGuid(), account, null, moneyToTake);
            transaction.Cancel();
            _transactions.Add(transaction);
            account.BankBelonging.AddTransaction(transaction);
        }
    }

    public void TransferMoneyBetweenAccounts(IBankAccount accountSender, IBankAccount accountRecipient,  decimal moneyToTransfer)
    {
        if (accountSender is null)
            throw new BanksException("Account of sender is null");
        if (accountRecipient is null)
            throw new BanksException("Account of recipient is null");
        if (moneyToTransfer < Constans.MinAmountOfMoney)
            throw new BanksException("Input larger sum");
        if (accountSender.IsTransactionPossible(accountSender.Owner, accountRecipient.Owner))
        {
            Transaction transaction = new Transaction(Guid.NewGuid(), accountSender, accountRecipient, moneyToTransfer);
            accountSender.Withdraw(moneyToTransfer);
            accountRecipient.FillUp(moneyToTransfer);
            _transactions.Add(transaction);
            accountSender.BankBelonging.AddTransaction(transaction);
            accountRecipient.BankBelonging.AddTransaction(transaction);
        }
        else
        {
            Transaction transaction = new Transaction(Guid.NewGuid(), accountSender, accountRecipient, moneyToTransfer);
            transaction.Cancel();
            _transactions.Add(transaction);
            accountSender.BankBelonging.AddTransaction(transaction);
            accountRecipient.BankBelonging.AddTransaction(transaction);
        }
    }

    public void SkipTime(int days)
    {
        if (days < Constans.MinTerm)
            throw new BanksException("Amount of days to skip must be at least 1");
        foreach (var bank in _banks)
        {
            bank.UpdateAccounts(days);
        }
    }
}