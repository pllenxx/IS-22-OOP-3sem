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

    public void CheckTransaction(Transaction transaction)
    {
        if (transaction is null)
            throw new BanksException("Nothing to check");
        if (transaction.Sender.Owner.CheckDoubtfulness() && transaction.Recipient is null)
        {
            CancelTransaction(transaction);
        }
        else if (transaction.Sender.Owner.CheckDoubtfulness() && transaction.Recipient is not null && transaction.Recipient.Owner.CheckDoubtfulness())
        {
            CancelTransaction(transaction);
        }
        else
        {
            _transactions.Add(transaction);
        }
    }

    public void CancelTransaction(Transaction transaction)
    {
        if (transaction is null)
            throw new BanksException("Nothing to cancel");
        transaction.Cancel();
        transaction.Sender.FillUp(transaction.AmountOfMoney);
        if (transaction.Recipient is not null)
            transaction.Recipient.Withdraw(transaction.AmountOfMoney);
    }

    public void SkipTime(int days)
    {
        foreach (var bank in _banks)
        {
            bank.UpdateAccounts(days);
        }
    }
}