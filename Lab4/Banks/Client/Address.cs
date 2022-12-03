using Banks.Tools;

namespace Banks;

public class Address
{
    private const int MailCodeLength = 6;

    private string _city;
    private string _street;
    private string _houseNumber;
    private int _mailCode;

    public Address(string city, string street, string houseNumber, int mailCode)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new BanksException("City is null");
        if (string.IsNullOrWhiteSpace(street))
            throw new BanksException("Street is null");
        if (string.IsNullOrWhiteSpace(houseNumber))
            throw new BanksException("House number is null");
        if (mailCode.ToString().Length != MailCodeLength)
            throw new BanksException("Incorrect mail code input");
        _city = city;
        _street = street;
        _houseNumber = houseNumber;
        _mailCode = mailCode;
    }
}