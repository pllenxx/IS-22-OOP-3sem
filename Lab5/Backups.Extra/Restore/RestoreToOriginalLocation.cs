using Backups.Extra.Tools;

namespace Backups.Extra.Recover;

public class RestoreToOriginalLocation : IRestore
{
    private RestorePoint _point;

    public RestoreToOriginalLocation(RestorePoint point)
    {
        if (point is null)
            throw new BackupsExtraException("Point to restore is null");
        _point = point;
    }

    public void RestoreObjects(BackupExtraTask task, ILogger logger)
    {
        if (task is null)
            throw new BackupsExtraException("Backup task is null");
        if (logger is null)
            throw new BackupsExtraException("Logger is null");
        foreach (var storage in _point.Storages)
        {
            foreach (var obj in storage.BackupObjects)
            {
                File.Delete(obj.Path);
                var bytes = File.ReadAllBytes(Path.Combine(task.BackupPath, task.BackupName, _point.Name, storage.Path));
                File.WriteAllBytes(obj.Path, bytes);
            }
        }

        logger.Logging($"Objects from {_point.Name} were restored to original location");
    }
}