using Backups.Services;
using Backups.Tools;

namespace Backups;

public class SplitSaver : IStorageAlgorithm
{
    public IEnumerable<Storage> Save(string path, IEnumerable<BackupObject> objects)
    {
        if (string.IsNullOrEmpty(path))
            throw new BackupException("Invalid restore point path input");
        if (!objects.Any())
            throw new BackupException("Nothing to backup");
        var storages = new List<Storage>();
        foreach (var obj in objects)
        {
            IEnumerable<BackupObject> objectToAdd = new[] { obj };
            var storage = new Storage(obj.Path, objectToAdd);
            storages.Add(storage);
        }

        return storages.AsEnumerable();
    }
}