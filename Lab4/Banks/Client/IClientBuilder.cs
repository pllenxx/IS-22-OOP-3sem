namespace Banks;

public interface IClientBuilder
{
    void SetName(FullName name);
    void SetAddress(Address? address);
    void SetPassport(PassportData? passportNumber);
    Client BuildClient();
}