using Banks.Tools;
namespace Banks;

public class Validate
{
    protected internal Validate()
    {
    }

    public void CheckParams(
        double debitPercentage,
        double lowDepositPercentage,
        decimal lowDepositSum,
        double averageDepositPercentage,
        decimal averageDepositSum,
        double highDepositPercentage,
        decimal creditLimit,
        double commissionForCreditUse,
        decimal authorizedCapital)
    {
        if (debitPercentage < Constans.MinDebitPercentage)
            throw new BanksException("Percentage for debit is too little");
        if (lowDepositPercentage < Constans.MinLowDepositPercentage)
            throw new BanksException("Percentage for deposit account is too little");
        if (averageDepositPercentage < Constans.MinAverageDepositPercentage)
            throw new BanksException("Percentage for deposit account is too little");
        if (highDepositPercentage is < Constans.MinHighDepositPercentage or > Constans.MaxHighDepositPercentage)
            throw new BanksException("Percentage for deposit account isn't correct");
        if (creditLimit < Constans.MinCreditLimit)
            throw new BanksException("Credit limit is too little");
        if (commissionForCreditUse < Constans.MinCreditCommission)
            throw new BanksException("Commission for credit account is too little");
        if (authorizedCapital < Constans.MinMoneyToCreateBank)
            throw new BanksException("Not enough money to create a bank");
        if (lowDepositSum < Constans.MinLowDepositSum)
            throw new BanksException("Deposit sum is too little");
        if (averageDepositSum < Constans.MinAverageDepositSum)
            throw new BanksException("Deposit sum is too little");
    }
}