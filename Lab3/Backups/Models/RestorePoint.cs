using Backups.Tools;

namespace Backups;

public class RestorePoint
{
    private ICollection<Storage> _storages;

    public RestorePoint(DateTime time, ICollection<Storage> storages)
    {
        if (!storages.Any())
            throw new BackupException("No storages to create a restore point");
        TimeOfCreation = time;
        Name = $"RestorePoint№{Guid.NewGuid()}";
        _storages = storages;
    }

    public string Name { get; }
    public DateTime TimeOfCreation { get;  }

    public void AddStorages(IEnumerable<Storage> storages)
    {
        if (!storages.Any())
            throw new BackupException("Nothing to add to restore point");
        _storages = (ICollection<Storage>)storages;
        foreach (var storage in storages)
        {
            storage.SetPoint(this);
        }
    }
}