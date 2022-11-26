using Banks.Tools;

namespace Banks;

public abstract class BankAccountCreator
{
    public BankAccountCreator(Bank bank, Client client)
    {
        Client = client;
        BankBelonging = bank;
    }

    public Client Client { get; protected internal set; }
    public Bank BankBelonging { get; protected internal set; }

    public abstract IBankAccount CreateAccount();
}