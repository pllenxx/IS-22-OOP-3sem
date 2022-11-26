using Banks.Tools;

namespace Banks;

public interface IBankAccount
{
    Guid Id { get; }
    Client Owner { get; }
    Bank BankBelonging { get; }
    decimal Balance { get; }
    void Withdraw(decimal moneyToTake);
    void FillUp(decimal moneyToTopOff);
    void Transfer(decimal moneyToTransfer, IBankAccount account);
    void AddPercent();
}