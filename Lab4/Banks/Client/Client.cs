using Banks.Messages;
using Banks.Observer;
using Banks.Tools;

namespace Banks;

public class Client : IObserver
{
    private Guid _id;
    private FullName _name;
    private Address? _address;
    private List<IObservable> _subscriptions;
    private List<IMessage> _messages;

    protected internal Client(FullName name, Address? address, PassportData? data)
    {
        if (name is null)
            throw new BanksException("Client's name cannot be null");
        _id = Guid.NewGuid();
        _name = name;
        _address = address;
        Passport = data;
        _subscriptions = new List<IObservable>();
        _messages = new List<IMessage>();
    }

    public FullName FullName => _name;
    public PassportData? Passport { get; private set; }
    public IReadOnlyList<IMessage> Messages => _messages.AsReadOnly();

    public void UpdateAddress(Address address)
    {
        if (address is null)
            throw new BanksException("Unable to update address because it's null");
        _address = address;
    }

    public void UpdatePassportData(PassportData passportData)
    {
        if (passportData is null)
            throw new BanksException("Unable to update passport info because it's null");
        Passport = passportData;
    }

    public void ConfirmSubscription()
    {
        CentralBank centralBank = CentralBank.GetInstance();
        foreach (var bank in centralBank.Banks)
        {
            if (bank.Clients.Contains(this))
            {
                bank.AddObserver(this);
                _subscriptions.Add(bank);
            }
        }
    }

    public bool CheckDoubtfulness()
    {
        return Passport is null;
    }

    public void Update(IMessage message)
    {
        _messages.Add(message);
    }
}