namespace Backups.Extra.Remover;

public interface ILimitAlgorithm
{
    IEnumerable<RestorePoint> FindPoints(IEnumerable<RestorePoint> restorePoints);
}