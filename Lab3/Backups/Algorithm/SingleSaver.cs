using Backups.Services;
using Backups.Tools;

namespace Backups;

public class SingleSaver : IStorageAlgorithm
{
    public IEnumerable<Storage> Save(string path, IEnumerable<BackupObject> objects)
    {
        if (string.IsNullOrEmpty(path))
            throw new BackupException("Invalid restore point path input");
        if (!objects.Any())
            throw new BackupException("Nothing to backup");
        var storages = new List<Storage>(1);
        var id = Guid.NewGuid();
        var storagePath = Path.Combine(path, $"Archive{id}.zip");
        var storage = new Storage(storagePath, objects);
        storages.Add(storage);
        return storages.AsEnumerable();
    }
}