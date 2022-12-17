using Backups.Tools;

namespace Backups;

public class Backup : IBackup
{
    private List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public IReadOnlyList<RestorePoint> GetPoints()
    {
        return _restorePoints.AsReadOnly();
    }

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint is null)
            throw new BackupException("Restore point is null");
        _restorePoints.Add(restorePoint);
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        _restorePoints.Remove(restorePoint);
    }
}