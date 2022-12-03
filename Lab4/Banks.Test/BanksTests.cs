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
        var client = builder.BuildClient();
        var account = registeredBank.CreateDebitAccount(client, 60000);
        centralBank.AddMoneyToAccount(account, 5000);
        builder.SetName(new FullName("Vika", "Butcher"));
        builder.SetAddress(new Address("Moskva", "Bumblebee", "228", 198206));
        builder.SetPassport(new PassportData(8888, 999999));
        var newClient = builder.BuildClient();
        var newAcc = registeredBank.CreateDebitAccount(newClient, 1000000);
        centralBank.ReduceMoneyFromAccount(newAcc, 100);
        centralBank.TransferMoneyBetweenAccounts(account, newAcc, 2);
        Assert.Equal(1000000 - 100, newAcc.Balance);
        Assert.True(centralBank.Transactions.Last().IsCanceled);
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
            12.5,
            1500000000);
        Bank registeredBank = centralBank.RegisterBank("Sberspozhalusta", settings);
        IClientBuilder builder = new ClientBuilder();
        builder.SetName(new FullName("Polina", "Khartanovich"));
        var client1 = builder.BuildClient();
        registeredBank.CreateDebitAccount(client1, 100000);
        client1.ConfirmSubscription();
        registeredBank.ChangeConditionsForDebit(10);
        registeredBank.CreateDepositAccount(client1, 90000, DateTime.Now, 12);
        registeredBank.ChangeLowPercentForDeposit(5);
        BankSettings newSettings = new BankSettings(
            4,
            5,
            35000,
            8,
            60000,
            9,
            100000,
            15,
            1500000000);
        Bank newRegisteredBank = centralBank.RegisterBank("Sbernezachto", newSettings);
        newRegisteredBank.CreateCreditAccount(client1);
        client1.ConfirmSubscription();
        newRegisteredBank.ChangeCreditLimit(200000);
        builder.SetName(new FullName("Dab", "Step"));
        var client2 = builder.BuildClient();
        newRegisteredBank.CreateDebitAccount(client2, 200000);
        newRegisteredBank.ChangeConditionsForDebit(5);
        Assert.True(client1.Messages.Count == 4);
        Assert.True(client2.Messages.Count == 0);
    }
}