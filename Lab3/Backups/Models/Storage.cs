using System.Net;
using Backups.Tools;

namespace Backups;

public class Storage
{
    private RestorePoint _point = null!;
    private List<BackupObject> _backupObjects;

    public Storage(string path, List<byte[]> bytes)
    {
        if (string.IsNullOrEmpty(path))
            throw new BackupException("Storage address is incorrect");
        if (!bytes.Any())
            throw new BackupException("Content is empty");
        _backupObjects = new List<BackupObject>();
        Path = path;
        Bytes = bytes;
    }

    public string Path { get; }
    public List<byte[]> Bytes { get; }

    public void SetPoint(RestorePoint point)
    {
        if (point is null)
            throw new BackupException("Restore point does not exist");
        _point = point;
    }

    public void AddObject(BackupObject backupObject)
    {
        if (backupObject is null)
            throw new BackupException("Object is null");
        _backupObjects.Add(backupObject);
    }
}