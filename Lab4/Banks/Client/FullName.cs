using Banks.Tools;

namespace Banks;

public class FullName
{
    private string _name;
    private string _surname;

    public FullName(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Client's name is incorrect");
        if (string.IsNullOrWhiteSpace(surname))
            throw new BanksException("Client's surname is incorrect");
        _name = name;
        _surname = surname;
    }
}