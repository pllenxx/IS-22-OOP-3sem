using Backups.Services;
using Backups.Tools;

namespace Backups;

public class SplitSaver : IStorageAlgorithm
{
    public IEnumerable<Storage> Save(BackupTask task)
    {
        if (!task.Objects.Any())
            throw new BackupException("Nothing to backup");
        var storages = new List<Storage>();
        foreach (var obj in task.Objects)
        {
            var archiver = new ZipArchiver();
            IReadOnlyList<BackupObject> objectToZip = new[] { obj };
            List<byte[]> bytes = new List<byte[]>(1);
            var content = archiver.GetContent(objectToZip);
            bytes.Add(content);
            var storage = new Storage($"Archive_{Guid.NewGuid()}.zip", bytes.AsReadOnly());
            storage.AddObject(obj);
            storages.Add(storage);
        }

        return storages;
    }
}