namespace Backups;

public interface IBackup
{
    void AddRestorePoint(RestorePoint restorePoint);
}