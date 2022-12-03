using Banks.Tools;

namespace Banks;

public abstract class BankAccountCreator
{
    protected BankAccountCreator(Bank bank, Client client)
    {
        if (bank is null)
            throw new BanksException("Unable to create account with null bank");
        if (client is null)
            throw new BanksException("Unable to create account with null client");
        Client = client;
        BankBelonging = bank;
    }

    public Client Client { get; protected internal set; }
    public Bank BankBelonging { get; protected internal set; }

    public abstract IBankAccount CreateAccount();
}