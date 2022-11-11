using Backups.Tools;

namespace Backups;

public class Backup : IBackup
{
    private ICollection<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint is null)
            throw new BackupException("Restore point is null");
        _restorePoints.Add(restorePoint);
    }
}