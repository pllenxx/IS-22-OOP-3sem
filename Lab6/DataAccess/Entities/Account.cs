using DataAccess.Enums;
namespace DataAccess.Entities;

public class Account
{
    public Account(Guid id, string mainInfo, string passwordHash, AccountType type)
    {
        Id = id;
        MainInfo = mainInfo;
        Type = type;
        PasswordHash = passwordHash;
    }
    
    protected Account() {}
    public Guid Id { get; set; }
    public string MainInfo { get; set; }
    public string PasswordHash { get; set; }
    public AccountType Type { get; set; }
    public bool? AllowReportCreation { get; set; }
}