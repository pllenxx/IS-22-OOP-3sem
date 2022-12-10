using Backups.Extra.Tools;

namespace Backups.Extra.Remover;

public class HybridLimitAlgorithm : ILimitAlgorithm
{
    private AmountLimitAlgorithm? _byAmountRemover;
    private DateLimitAlgorithm? _byDateRemover;
    private HybridMode _mode;

    public HybridLimitAlgorithm(HybridMode mode, AmountLimitAlgorithm? byAmountRemover, DateLimitAlgorithm? byDateRemover)
    {
        if (byAmountRemover is null && byDateRemover is null)
            throw new BackupsExtraException("Both limit criteria is null");
        _mode = mode;
        _byAmountRemover = byAmountRemover ?? null;
        _byDateRemover = byDateRemover ?? null;
    }

    public IEnumerable<RestorePoint> FindPoints(IEnumerable<RestorePoint> restorePoints)
    {
        if (!restorePoints.Any())
            throw new BackupsExtraException("No points to find");
        IEnumerable<RestorePoint> pointsForAmountCriteria = new List<RestorePoint>();
        IEnumerable<RestorePoint> pointsForDateCriteria = new List<RestorePoint>();
        IEnumerable<RestorePoint> resultPoints = new List<RestorePoint>();
        if (_mode.Any)
        {
            if (_byAmountRemover is not null)
                pointsForAmountCriteria = _byAmountRemover.FindPoints(restorePoints);

            if (_byDateRemover is not null)
                pointsForDateCriteria = _byDateRemover.FindPoints(restorePoints);

            resultPoints = pointsForAmountCriteria.Concat(pointsForDateCriteria);
        }

        if (_mode.All)
        {
            if (_byAmountRemover is not null)
                pointsForAmountCriteria = _byAmountRemover.FindPoints(restorePoints);

            if (_byDateRemover is not null)
                pointsForDateCriteria = _byDateRemover.FindPoints(restorePoints);

            resultPoints = pointsForAmountCriteria.Intersect(pointsForDateCriteria);
        }

        return resultPoints;
    }
}