using Banks.Tools;

namespace Banks;

public class Transaction
{
    private Guid _id;
    private bool _isCanceled;

    public Transaction(IBankAccount sender, IBankAccount? recipient, decimal amountOfMoney)
    {
        if (sender is null)
            throw new BanksException("Sender is null");
        _id = Guid.NewGuid();
        Sender = sender;
        Recipient = recipient;
        AmountOfMoney = amountOfMoney;
        _isCanceled = false;
    }

    public IBankAccount Sender { get; private set; }
    public IBankAccount? Recipient { get; private set; }
    public decimal AmountOfMoney { get; private set; }

    public bool IsCanceled => _isCanceled;
    public void Cancel()
    {
        _isCanceled = true;
    }
}