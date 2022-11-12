using Backups.Services;
using Backups.Tools;

namespace Backups;

public class SingleSaver : IStorageAlgorithm
{
    public List<Storage> Save(BackupTask task)
    {
        if (!task.Objects.Any())
            throw new BackupException("Nothing to backup");
        var storages = new List<Storage>(1);
        var archiver = new ZipArchiver();
        List<byte[]> filesContent = task.Objects.Select(obj => new[] { obj }).Select(objectToZip => archiver.GetContent(objectToZip)).ToList();

        var storage = new Storage($"Archive_{Guid.NewGuid()}.zip", filesContent);

        foreach (var obj in task.Objects)
        {
            storage.AddObject(obj);
        }

        storages.Add(storage);

        return storages;
    }
}