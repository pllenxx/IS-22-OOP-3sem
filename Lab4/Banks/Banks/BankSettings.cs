using Banks.Tools;

namespace Banks;

public class BankSettings
{
    private readonly Validate _validator = new Validate();
    public BankSettings(
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
        _validator.CheckParams(
            debitPercentage,
            lowDepositPercentage,
            lowDepositSum,
            averageDepositPercentage,
            averageDepositSum,
            highDepositPercentage,
            creditLimit,
            commissionForCreditUse,
            authorizedCapital);
        DebitPercentage = debitPercentage;
        LowDepositPercentage = lowDepositPercentage;
        AverageDepositPercentage = averageDepositPercentage;
        HighDepositPercentage = highDepositPercentage;
        CreditLimit = creditLimit;
        CommissionForCreditUse = commissionForCreditUse;
        AuthorizedCapital = authorizedCapital;
        LowDepositSum = lowDepositSum;
        AverageDepositSum = averageDepositSum;
    }

    public double DebitPercentage { get; private set; }
    public double LowDepositPercentage { get; private set; }
    public decimal LowDepositSum { get; private set; }
    public double AverageDepositPercentage { get; private set; }
    public decimal AverageDepositSum { get; private set; }
    public double HighDepositPercentage { get; private set; }
    public decimal CreditLimit { get; private set; }
    public double CommissionForCreditUse { get; private set; }
    public decimal AuthorizedCapital { get; private set; }

    public void UpdateDebitPercentage(double newPercent)
    {
        if (newPercent < Constans.MinDebitPercentage)
            throw new BanksException("Percent cannot be negative");
        DebitPercentage = newPercent;
    }

    public void UpdateLowDepositPercentage(double newPercent)
    {
        if (newPercent < Constans.MinLowDepositPercentage)
            throw new BanksException("Percent cannot be negative");
        LowDepositPercentage = newPercent;
    }

    public void UpdateAverageDepositPercentage(double newPercent)
    {
        if (newPercent < Constans.MinAverageDepositPercentage)
            throw new BanksException("Percent cannot be negative");
        AverageDepositPercentage = newPercent;
    }

    public void UpdateHighDepositPercentage(double newPercent)
    {
        if (newPercent < Constans.MinHighDepositPercentage)
            throw new BanksException("Percent cannot be negative");
        HighDepositPercentage = newPercent;
    }

    public void UpdateCreditLimit(decimal newLimit)
    {
        if (newLimit < Constans.MinCreditLimit)
            throw new BanksException("Money limit cannot be negative");
        CreditLimit = newLimit;
    }

    public void UpdateCommissionForCreditUse(double newCommission)
    {
        if (newCommission < Constans.MinCreditCommission)
            throw new BanksException("Commission cannot be negative");
        CommissionForCreditUse = newCommission;
    }
}