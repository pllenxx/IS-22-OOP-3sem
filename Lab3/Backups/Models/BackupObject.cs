namespace Backups;

public class BackupObject
{
    public BackupObject(string path)
    {
        // if (string.IsNullOrEmpty(name))
        //     throw new Exception("Object must have name");
        if (string.IsNullOrEmpty(path))
            throw new Exception("Object must have path");
        Id = Guid.NewGuid();
        Path = path;
    }

    public Guid Id { get; }
    public string Path { get; }
}