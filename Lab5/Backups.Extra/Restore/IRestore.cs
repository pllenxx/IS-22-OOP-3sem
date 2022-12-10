namespace Backups.Extra.Recover;

public interface IRestore
{
    void RestoreObjects(BackupExtraTask task, ILogger logger);
}