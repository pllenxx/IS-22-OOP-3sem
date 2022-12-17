using System.IO.Compression;
using Backups.Extra.Tools;

namespace Backups.Extra.Recover;

public class RestoreToDifferentLocation : IRestore
{
    private string _newLocation;
    private RestorePoint _point;

    public RestoreToDifferentLocation(string path, RestorePoint point)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsExtraException("Incorrect path input");
        if (point is null)
            throw new BackupsExtraException("Point to get objects is null");
        _newLocation = path;
        _point = point;
    }

    public void RestoreObjects(BackupExtraTask task, ILogger logger)
    {
        if (task is null)
            throw new BackupsExtraException("Backup task is null");
        if (logger is null)
            throw new BackupsExtraException("Logger is null");
        Directory.CreateDirectory(_newLocation);
        foreach (var storage in _point.Storages)
        {
            ZipFile.ExtractToDirectory(Path.Combine(task.BackupPath, task.BackupName, _point.Name, storage.Path), _newLocation);
        }

        logger.Logging($"{_point.Name} was restored to {_newLocation}");
    }
}