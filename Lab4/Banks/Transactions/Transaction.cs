using Banks.Tools;

namespace Banks;

public class Transaction
{
    public Transaction(Guid id, IBankAccount sender, IBankAccount? recipient, decimal amountOfMoney)
    {
        if (sender is null)
            throw new BanksException("Sender is null");
        Id = id;
        Sender = sender;
        Recipient = recipient;
        AmountOfMoney = amountOfMoney;
        IsCanceled = false;
    }

    public Guid Id { get; private set; }
    public IBankAccount Sender { get; private set; }
    public IBankAccount? Recipient { get; private set; }
    public decimal AmountOfMoney { get; private set; }

    public bool IsCanceled { get; private set; }

    public void Cancel()
    {
        IsCanceled = true;
    }
}