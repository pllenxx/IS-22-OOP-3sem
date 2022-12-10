using Backups.Extra.Tools;

namespace Backups.Extra.Remover;

public class AmountLimitAlgo : ILimitAlgo
{
    private const int MinAmountLimit = 0;
    private int _amountLimit;

    public AmountLimitAlgo(int amountLimit)
    {
        if (amountLimit < MinAmountLimit)
            throw new BackupsExtraException("Unable to collect negative amount of restore points");
        _amountLimit = amountLimit;
    }

    public IEnumerable<RestorePoint> FindPoints(IEnumerable<RestorePoint> restorePoints)
    {
        if (!restorePoints.Any())
            throw new BackupsExtraException("No points to find");
        return restorePoints.OrderBy(point => point.TimeOfCreation).Take(restorePoints.Count() - _amountLimit);
    }
}