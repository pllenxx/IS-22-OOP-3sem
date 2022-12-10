using Backups.Extra.Tools;

namespace Backups.Extra.Remover;

public class DateLimitAlgo : ILimitAlgo
{
    private DateTime _dateLimit;
    public DateLimitAlgo(DateTime dateLimit)
    {
        _dateLimit = dateLimit;
    }

    public IEnumerable<RestorePoint> FindPoints(IEnumerable<RestorePoint> restorePoints)
    {
        if (!restorePoints.Any())
            throw new BackupsExtraException("No points to find");
        return restorePoints.Where(restorePoint => restorePoint.TimeOfCreation < _dateLimit).ToList();
    }
}