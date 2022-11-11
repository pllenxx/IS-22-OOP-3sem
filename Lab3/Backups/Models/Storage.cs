using System.Net;

namespace Backups;

public class Storage
{
    private RestorePoint _point = null!;
    private IEnumerable<BackupObject> _backupObjects;

    public Storage(string path, IEnumerable<BackupObject> objects)
    {
        if (string.IsNullOrEmpty(path))
            throw new Exception("Storage address is incorrect");
        _backupObjects = objects;
        Path = path;
    }

    public string Path { get; }
    public void SetPoint(RestorePoint point)
    {
        if (point is null)
            throw new Exception("Restore point does not exist");
        _point = point;
    }
}