using Banks.Tools;
using Xunit;
namespace Banks.Test;

public class BanksTests
{
    private CentralBank centralBank = CentralBank.GetInstance();
    [Fact]
    public void CreateBank_BankCreated()
    {
        BankSettings settings = new BankSettings(
            7.9,
            3,
            50000,
            5,
            100000,
            7,
            50000,
            12,
            2000000000);
        Bank registeredBank = centralBank.RegisterBank("TakeMyMoney", settings);
        Assert.Contains(registeredBank, centralBank.Banks);
        Assert.Throws<BanksException>(() => settings = new BankSettings(
            7.9,
            3,
            50000,
            5,
            100000,
            7,
            50000,
            12,
            20000));
    }

    [Fact]
    public void CreateAccountForClient_BankHasAccount()
    {
        IClientBuilder builder = new ClientBuilder();
        builder.SetName(new FullName("Eldar", "Kasymov"));
        Client client = builder.BuildClient();
        BankSettings settings = new BankSettings(
            7.9,
            3,
            50000,
            5,
            100000,
            7,
            40000,
            12,
            2000000000);
        Bank registeredBank = centralBank.RegisterBank("TakeMyMoney", settings);
        var account = registeredBank.CreateDebitAccount(client, 100000000);
        Assert.Contains(client, registeredBank.Clients);
        Assert.Contains(account, registeredBank.Accounts);
        Assert.Equal(2000000000 + 100000000, registeredBank.BankMoney);
    }

    [Fact]
    public void PerformWithdraw_CancelledTransfer()
    {
        BankSettings settings = new BankSettings(
            8,
            3,
            35000,
            5,
            60000,
            7,
            100000,
            12,
            1500000000);
        Bank registeredBank = centralBank.RegisterBank("Nyam-nyam", settings);
        IClientBuilder builder = new ClientBuilder();
        builder.SetName(new FullName("Mama", "Papa"));
        Client client1 = builder.BuildClient();
        var account1 = registeredBank.CreateDebitAccount(client1, 100);
        account1.Withdraw(50);
        account1.FillUp(300);
        builder.SetName(new FullName("Papa", "Mama"));
        var client2 = builder.BuildClient();
        var account2 = registeredBank.CreateDebitAccount(client2, 200);
        account1.Transfer(20, account2);
        Assert.Equal(350, account1.Balance);
        Assert.True(registeredBank.Transactions.Last().IsCanceled);
    }

    [Fact]
    public void SkipTimeTest()
    {
        BankSettings settings = new BankSettings(
            3.65,
            3,
            35000,
            5,
            60000,
            7,
            100000,
            12,
            1500000000);
        Bank registeredBank = centralBank.RegisterBank("Sberspasibo", settings);
        IClientBuilder builder = new ClientBuilder();
        builder.SetName(new FullName("Polina", "Khartanovich"));
        var client = builder.BuildClient();
        var account = registeredBank.CreateDebitAccount(client, 100000);
        centralBank.SkipTime(1);
        Assert.Equal(100010, account.Balance);
    }

    [Fact]
    public void ClientsGetUpdates()
    {
        BankSettings settings = new BankSettings(
            3.65,
            3,
            35000,
            5,
            60000,
            7,
            100000,
            12,
            1500000000);
        Bank registeredBank = centralBank.RegisterBank("Sberspozhalusta", settings);
        IClientBuilder builder = new ClientBuilder();
        builder.SetName(new FullName("Polina", "Khartanovich"));
        var client = builder.BuildClient();
        registeredBank.CreateDebitAccount(client, 100000);
        client.ConfirmSubscription();
        registeredBank.ChangeConditionsForDebit(10);
        registeredBank.CreateDepositAccount(client, 90000, DateTime.Now, 12);
        registeredBank.ChangeLowPercentForDeposit(5);
        Assert.True(client.Messages.Count == 2);
    }
}