namespace Backups;

public interface IBackup
{
    IReadOnlyList<RestorePoint> GetPoints();
    void AddRestorePoint(RestorePoint restorePoint);
    void DeleteRestorePoint(RestorePoint restorePoint);
}