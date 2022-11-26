using Banks.Tools;

namespace Banks;

public class PassportData
{
    private const int SeriesLength = 4;
    private const int NumberLength = 4;

    private int _series;
    private int _number;

    public PassportData(int series, int number)
    {
        if (series.ToString().Length != SeriesLength)
            throw new BanksException("Passport series is incorrect");
        if (number.ToString().Length != NumberLength)
            throw new BanksException("Passport number is incorrect");
        _series = series;
        _number = number;
    }
}