using Backups.Extra.Tools;

namespace Backups.Extra.Remover;

public class DateLimitAlgorithm : ILimitAlgorithm
{
    private DateTime _dateLimit;
    public DateLimitAlgorithm(string date)
    {
        if (!DateTime.TryParse(date, out _dateLimit))
            throw new BackupsExtraException("Date for limit is incorrect");
    }

    public IEnumerable<RestorePoint> FindPoints(IEnumerable<RestorePoint> restorePoints)
    {
        if (!restorePoints.Any())
            throw new BackupsExtraException("No points to find");
        return restorePoints.Where(restorePoint => restorePoint.TimeOfCreation < _dateLimit).ToList();
    }
}