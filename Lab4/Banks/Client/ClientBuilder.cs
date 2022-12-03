using Banks.Tools;

namespace Banks;

public class ClientBuilder : IClientBuilder
{
    private FullName _name = null!;
    private Address? _address;
    private PassportData? _passportData;

    public void SetName(FullName name)
    {
        if (name is null)
            throw new BanksException("Client's name cannot be null");
        _name = name;
    }

    public void SetAddress(Address? address)
    {
        _address = address;
    }

    public void SetPassport(PassportData? passportNumber)
    {
        _passportData = passportNumber;
    }

    public Client BuildClient()
    {
        return new Client(_name, _address, _passportData);
    }
}