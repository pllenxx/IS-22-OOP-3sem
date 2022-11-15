using Backups.Tools;

namespace Backups;

public class BackupObject
{
    public BackupObject(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupException("Object must have path");
        Id = Guid.NewGuid();
        Path = path;
    }

    public Guid Id { get; }
    public string Path { get; }
}