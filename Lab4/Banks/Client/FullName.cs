using Banks.Tools;

namespace Banks;

public class FullName : IEquatable<FullName>
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

    public override int GetHashCode() => HashCode.Combine(_name, _surname);
    public override bool Equals(object? obj)
    {
        if (obj is null)
            throw new BanksException("Object is null");
        return Equals(obj as FullName);
    }

    public bool Equals(FullName? other)
    {
        if (other is null)
            throw new BanksException("Nothing to compare with");
        return other != null && _name.Equals(other._name) && _surname.Equals(other._surname);
    }
}