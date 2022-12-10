namespace Backups.Extra.Remover;

public interface ILimitAlgo
{
    IEnumerable<RestorePoint> FindPoints(IEnumerable<RestorePoint> restorePoints);
}